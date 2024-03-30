using Eventify.Application.Bookings.Commands.CreateBooking;
using Eventify.Application.Bookings.Common.Errors;
using Eventify.Application.Bookings.Common.Mappings;
using Eventify.Application.Common.Mappings;
using Eventify.Contracts.Bookings.Responses;
using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Bookings.Services;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;
using Eventify.Domain.Users;
using Eventify.Domain.Users.ValueObjects;

namespace Eventify.Application.UnitTests.Bookings.Commands.CreateBooking;

[TestSubject(typeof(CreateBookingCommandHandler))]
public sealed class CreateBookingCommandHandlerTests
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IAttendeeRepository attendeeRepository = A.Fake<IAttendeeRepository>();
    private readonly IBookingRepository bookingRepository = A.Fake<IBookingRepository>();
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
    private readonly ITicketRepository ticketRepository = A.Fake<ITicketRepository>();
    private readonly IBookingService bookingService = A.Fake<IBookingService>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly CreateBookingCommand command = new()
    {
        TicketId = Guid.NewGuid()
    };

    private readonly UserId userId = Constants.UserId;

    private readonly Attendee attendee = Factories.Attendee.CreateAttendee();
    private readonly Event @event = Factories.Event.CreateEvent();
    private readonly Ticket ticket = Factories.Ticket.CreateTicketValue();

    private readonly CreateBookingCommandHandler sut;

    public CreateBookingCommandHandlerTests()
    {
        TypeAdapterConfig.GlobalSettings.Apply(new CommonMappings());
        TypeAdapterConfig.GlobalSettings.Apply(new BookingMappings());

        sut = new CreateBookingCommandHandler(
            user,
            attendeeRepository,
            bookingRepository,
            eventRepository,
            ticketRepository,
            bookingService);

        A.CallTo(() => user.Id)
            .Returns(userId);

        A.CallTo(() => attendeeRepository.GetByUserAsync(userId, cancellationToken))
            .Returns(attendee);

        A.CallTo(() => ticketRepository.GetAsync(command.TicketId, cancellationToken))
            .Returns(ticket);

        A.CallTo(() => eventRepository.GetAsync(ticket.EventId, cancellationToken))
            .Returns(@event);
    }

    [Fact]
    public async Task Handle_WhenAttendeeDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(null as Attendee);

        // Act
        var act = () => sut.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage("Attendee not found");
    }

    [Fact]
    public async Task Handle_WhenTicketDoesNotExist_ShouldReturnTicketNotFound()
    {
        // Arrange
        A.CallTo(() => ticketRepository.GetAsync(command.TicketId, cancellationToken))
            .Returns(null as Ticket);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeFailure(BookingErrors.TicketNotFound(command.TicketId));
    }

    [Fact]
    public async Task Handle_WhenTicketEventDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => eventRepository.GetAsync(ticket.EventId, cancellationToken))
            .Returns(null as Event);

        // Act
        var act = () => sut.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage($"Event {ticket.EventId} not found");
    }

    [Fact]
    public async Task Handle_WhenBookingCreationFails_ShouldReturnError()
    {
        // Arrange
        A.CallTo(() => bookingService.PlaceBooking(@event, ticket, attendee, A<IEnumerable<Booking>>._))
            .Returns(Error.Failure("Booking failed."));

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeFailure(Error.Failure("Booking failed."));
    }

    [Fact]
    public async Task Handle_WhenBookingCreationSucceeds_ShouldAddToRepository()
    {
        // Arrange
        var booking = Factories.Booking.CreateBooking();
        A.CallTo(() => bookingService.PlaceBooking(@event, ticket, attendee, A<IEnumerable<Booking>>._))
            .Returns(booking);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => bookingRepository.AddAsync(booking, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenBookingCreationSucceeds_ShouldReturnBookingResponse()
    {
        // Arrange
        var booking = Factories.Booking.CreateBooking();
        A.CallTo(() => bookingService.PlaceBooking(@event, ticket, attendee, A<IEnumerable<Booking>>._))
            .Returns(booking);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSuccess(booking.Adapt<BookingResponse>() with
        {
            Event = new BookingEventResponse
            {
                Id = @event.Id.Value,
                Name = @event.Details.Name,
                Location = @event.Location.ToString(),
                Start = @event.Period.Start,
                End = @event.Period.End
            },
            Ticket = new BookingTicketResponse
            {
                Id = ticket.Id.Value,
                Name = ticket.Name,
                Price = ticket.Price.Value
            }
        });
    }
}