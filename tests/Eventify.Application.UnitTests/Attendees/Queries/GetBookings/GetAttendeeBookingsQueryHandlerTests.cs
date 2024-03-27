using Eventify.Application.Attendees.Queries.GetBookings;
using Eventify.Contracts.Attendees.Responses;
using Eventify.Contracts.Common.Responses;
using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;
using Eventify.Domain.Users;
using Eventify.Domain.Users.ValueObjects;

namespace Eventify.Application.UnitTests.Attendees.Queries.GetBookings;

[TestSubject(typeof(GetAttendeeBookingsQueryHandler))]
public sealed class GetAttendeeBookingsQueryHandlerTests
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IAttendeeRepository attendeeRepository = A.Fake<IAttendeeRepository>();
    private readonly IBookingRepository bookingRepository = A.Fake<IBookingRepository>();
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
    private readonly ITicketRepository ticketRepository = A.Fake<ITicketRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly GetAttendeeBookingsQuery query = new()
    {
        Page = 1,
        Limit = 10
    };

    private readonly GetAttendeeBookingsQueryHandler sut;

    private static readonly Attendee Attendee = Factories.Attendee.CreateAttendee();
    private static readonly Event Event = Factories.Event.CreateEvent();
    private static readonly Ticket Ticket = Factories.Ticket.CreateTicketValue(eventId: Event.Id);

    private static readonly Booking Booking = Factories.Booking.CreateBooking(
        attendeeId: Attendee.Id,
        eventId: Event.Id,
        ticketId: Ticket.Id);

    private static readonly UserId UserId = Constants.UserId;

    public GetAttendeeBookingsQueryHandlerTests()
    {
        sut = new GetAttendeeBookingsQueryHandler(
            user,
            attendeeRepository,
            bookingRepository,
            eventRepository,
            ticketRepository);

        Event.AddTicket(Ticket);
        Attendee.AddBooking(Booking);

        A.CallTo(() => user.Id)
            .Returns(UserId);

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(Attendee);

        A.CallTo(() => bookingRepository.GetAsync(Booking.Id, cancellationToken))
            .Returns(Booking);

        A.CallTo(() => eventRepository.GetAsync(Booking.EventId, cancellationToken))
            .Returns(Event);

        A.CallTo(() => ticketRepository.GetAsync(Booking.TicketId, cancellationToken))
            .Returns(Ticket);
    }

    [Fact]
    public async Task Handle_WhenAttendeeDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(null as Attendee);

        // Act
        var act = () => sut.Handle(query, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ApplicationException>()
            .WithMessage("Attendee not found");
    }

    [Fact]
    public async Task Handle_WhenAttendeeExists_ShouldReturnAttendeeBookingPageResponse()
    {
        // Arrange
        // Act
        var result = await sut.Handle(query, cancellationToken);

        // Assert
        result.Value.Should().BeEquivalentTo(new PageResponse<AttendeeBookingResponse>
        {
            Page = query.Page,
            Limit = query.Limit,
            Total = 1,
            Items =
            [
                new AttendeeBookingResponse
                {
                    Id = Booking.Id.Value,
                    State = Booking.State.Name,
                    TotalPrice = Booking.TotalPrice.Value,
                    TotalQuantity = Booking.TotalQuantity.Value,
                    PlacedAt = Booking.PlacedAt,
                    Ticket = new AttendeeBookingTicketResponse
                    {
                        Id = Booking.TicketId.Value,
                        Name = Ticket.Name,
                        Price = Ticket.Price.Value
                    },
                    Event = new AttendeeBookingEventResponse
                    {
                        Id = Booking.EventId.Value,
                        Name = Event.Details.Name,
                        Start = Event.Period.Start,
                        End = Event.Period.End,
                        Location = Event.Location.ToString(),
                        PosterUrl = Event.PosterUrl?.ToString()
                    }
                }
            ]
        });
    }
}