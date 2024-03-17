using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Events;
using Eventify.Domain.Events.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Bookings.Events;

internal sealed class BookingConfirmedHandler(
    IAttendeeRepository attendeeRepository,
    IEventRepository eventRepository,
    ILogger<BookingConfirmedHandler> logger
) : IDomainEventHandler<BookingConfirmed>
{
    public async Task Handle(BookingConfirmed e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Event} domain event", nameof(BookingPlaced));

        await AddBookingToAttendeeAsync(e.Booking, cancellationToken);
        await AddBookingToEventAsync(e.Booking, cancellationToken);
    }

    private async Task AddBookingToAttendeeAsync(Booking booking, CancellationToken cancellationToken)
    {
        var attendee = await attendeeRepository.GetAsync(booking.AttendeeId, cancellationToken);
        if (attendee is null)
        {
            throw new ApplicationException($"Attendee {booking.AttendeeId} not found");
        }

        var result = await attendee.AddBooking(booking)
            .ThenAsync(_ => attendeeRepository.UpdateAsync(attendee, cancellationToken));

        if (result.IsError)
        {
            throw new ApplicationException($"Failed to add booking {booking.Id} to " +
                                           $"attendee {booking.AttendeeId}: {result.FirstError.Description}");
        }
    }

    private async Task AddBookingToEventAsync(Booking booking, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAsync(booking.EventId, cancellationToken);
        if (@event is null)
        {
            throw new ApplicationException($"Event {booking.EventId} not found");
        }

        var result = await @event.AddBooking(booking)
            .ThenAsync(_ => eventRepository.UpdateAsync(@event, cancellationToken));

        if (result.IsError)
        {
            throw new ApplicationException($"Failed to add booking {booking.Id} to " +
                                           $"event {booking.EventId}: {result.FirstError.Description}");
        }
    }
}