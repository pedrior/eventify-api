using Eventify.Domain.Attendees.Events;
using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Bookings.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Attendees.Events;

internal sealed class BookingCancellationRequestedHandler(
    IBookingRepository bookingRepository,
    ILogger<BookingCancellationRequestedHandler> logger
) : IDomainEventHandler<BookingCancellationRequested>
{
    public Task Handle(BookingCancellationRequested e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {EventName} domain event", nameof(BookingCancellationRequestedHandler));

        return ProcessBookingCancellationRequestAsync(e.BookingId, cancellationToken);
    }

    private async Task ProcessBookingCancellationRequestAsync(BookingId bookingId,
        CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetAsync(bookingId, cancellationToken);
        if (booking is null)
        {
            throw new ApplicationException($"Booking {bookingId} not found");
        }

        // Just accept the cancellation request
        var result = await booking.Cancel(CancellationReason.AttendeeRequest)
            .ThenAsync(() => bookingRepository.UpdateAsync(booking, cancellationToken));

        result.EnsureSuccess();
    }
}