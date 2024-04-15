using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Events;
using Eventify.Domain.Events.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Bookings.Events;

internal sealed class BookingConfirmedHandler(
    IEventRepository eventRepository,
    ILogger<BookingConfirmedHandler> logger
) : IDomainEventHandler<BookingConfirmed>
{
    public async Task Handle(BookingConfirmed e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Event} domain event", nameof(BookingPlaced));

        await AddBookingToEventAsync(e.Booking, cancellationToken);
    }

    private async Task AddBookingToEventAsync(Booking booking, CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAsync(booking.EventId, cancellationToken);
        if (@event is null)
        {
            throw new ApplicationException($"Event {booking.EventId} not found");
        }

        await @event.AddBooking(booking)
            .ThrowIfFailure()
            .ThenAsync(() => eventRepository.UpdateAsync(@event, cancellationToken));
    }
}