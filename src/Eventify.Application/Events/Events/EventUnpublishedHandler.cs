using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Events.Events;
using Microsoft.Extensions.Logging;
using EventId = Eventify.Domain.Events.ValueObjects.EventId;

namespace Eventify.Application.Events.Events;

internal sealed class EventUnpublishedHandler(
    IBookingRepository bookingRepository,
    ILogger<EventUnpublishedHandler> logger
) : IDomainEventHandler<EventUnpublished>
{
    public Task Handle(EventUnpublished e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {EventName} domain event", nameof(EventUnpublished));

        return CancelAllEventBookingsAsync(e.EventId, cancellationToken);
    }

    private async Task CancelAllEventBookingsAsync(EventId eventId, CancellationToken cancellationToken)
    {
        foreach (var booking in await bookingRepository.ListByEventAsync(eventId, cancellationToken))
        {
            var result = await booking.Cancel(CancellationReason.EventCancelled)
                .ThenAsync(_ => bookingRepository.UpdateAsync(booking, cancellationToken));
            
            if (result.IsError)
            {
                logger.LogError("Error canceling booking {BookingId} for event {EventId}: {@Error}",
                    booking.Id, eventId, result.FirstError);
            }
        }
    }
}