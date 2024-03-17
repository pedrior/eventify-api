using Eventify.Application.Bookings.Common.Errors;
using Eventify.Application.Bookings.Common.Mappings;
using Eventify.Application.Bookings.Queries.GetBooking;
using Eventify.Application.Common.Mappings;
using Eventify.Contracts.Bookings.Responses;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;

namespace Eventify.Application.UnitTests.Bookings.Queries.GetBooking;

[TestSubject(typeof(GetBookingQueryHandler))]
public sealed class GetBookingQueryHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
    private readonly IBookingRepository bookingRepository = A.Fake<IBookingRepository>();
    private readonly ITicketRepository ticketRepository = A.Fake<ITicketRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly Booking booking = Factories.Booking.CreateBooking();
    private readonly Event @event = Factories.Event.CreateEvent();
    private readonly Ticket ticket = Factories.Ticket.CreateTicketValue();

    private readonly GetBookingQuery query = new()
    {
        BookingId = Guid.NewGuid()
    };

    private readonly GetBookingQueryHandler sut;

    public GetBookingQueryHandlerTests()
    {
        TypeAdapterConfig.GlobalSettings.Apply(new CommonMappings());
        TypeAdapterConfig.GlobalSettings.Apply(new BookingMappings());

        sut = new GetBookingQueryHandler(eventRepository, bookingRepository, ticketRepository);

        A.CallTo(() => bookingRepository.GetAsync(query.BookingId, cancellationToken))
            .Returns(booking);

        A.CallTo(() => eventRepository.GetAsync(booking.EventId, cancellationToken))
            .Returns(@event);

        A.CallTo(() => ticketRepository.GetAsync(booking.TicketId, cancellationToken))
            .Returns(ticket);
    }

    [Fact]
    public async Task Handle_WhenBookingDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        A.CallTo(() => bookingRepository.GetAsync(query.BookingId, cancellationToken))
            .Returns(null as Booking);

        // Act
        var result = await sut.Handle(query, cancellationToken);

        // Assert
        result.Should().BeError(BookingErrors.NotFound(query.BookingId));
    }

    [Fact]
    public async Task Handle_WhenBookingReferencesNonExistingEvent_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => eventRepository.GetAsync(booking.EventId, cancellationToken))
            .Returns(null as Event);

        // Act
        var act = () => sut.Handle(query, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage($"Event {booking.EventId} not found");
    }

    [Fact]
    public async Task Handle_WhenBookingReferencesNonExistingTicket_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => ticketRepository.GetAsync(booking.TicketId, cancellationToken))
            .Returns(null as Ticket);

        // Act
        var act = () => sut.Handle(query, cancellationToken);
        
        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage($"Ticket {booking.TicketId} not found");
    }

    [Fact]
    public async Task Handle_WhenBookingExists_ShouldReturnBookingResponse()
    {
        // Arrange
        // Act
        var result = await sut.Handle(query, cancellationToken);

        // Assert
        result.Should().BeValue(booking.Adapt<BookingResponse>() with
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