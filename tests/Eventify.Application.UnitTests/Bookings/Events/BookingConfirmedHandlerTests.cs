using Eventify.Application.Bookings.Events;
using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Events;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Bookings.Events;

[TestSubject(typeof(BookingConfirmedHandler))]
public sealed class BookingConfirmedHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
    private readonly ILogger<BookingConfirmedHandler> logger = A.Fake<ILogger<BookingConfirmedHandler>>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();
    
    private readonly Booking booking = Factories.Booking.CreateBooking();
    private readonly Event @event = Factories.Event.CreateEvent();

    private readonly BookingConfirmed e;

    private readonly BookingConfirmedHandler sut;

    public BookingConfirmedHandlerTests()
    {
        e = new BookingConfirmed(booking);
        
        @event.AddTicket(Factories.Ticket.CreateTicketValue(ticketId: booking.TicketId));
        @event.Publish();

        sut = new BookingConfirmedHandler(eventRepository, logger);

        A.CallTo(() => eventRepository.GetAsync(booking.EventId, cancellationToken))
            .Returns(@event);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldAddBookingToEvent()
    {
        // Arrange
        // Act
        await sut.Handle(e, cancellationToken);

        // Assert
        A.CallTo(() => eventRepository.UpdateAsync(
                A<Event>.That.Matches(e1 => e1.BookingIds.Contains(booking.Id)), cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenEventDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => eventRepository.GetAsync(booking.EventId, cancellationToken))
            .Returns(null as Event);

        // Act
        var act = () => sut.Handle(e, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage($"Event {booking.EventId} not found");
    }

    [Fact]
    public async Task Handle_WhenAddBookingToEventFails_ShouldThrowApplicationException()
    {
        // Arrange
        @event.AddBooking(booking);

        // Act
        var act = () => sut.Handle(e, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage($"Failed to add booking {booking.Id} to event {booking.EventId}: *");
    }
}