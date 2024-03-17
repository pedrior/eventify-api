using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.Events;
using Eventify.Domain.Bookings.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Bookings.Events;

internal sealed class BookingPlacedHandler(
    IBookingRepository bookingRepository,
    ILogger<BookingPlacedHandler> logger
) : IDomainEventHandler<BookingPlaced>
{
    public Task Handle(BookingPlaced e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Event} domain event", nameof(BookingPlaced));

        return ProcessBookingPaymentAsync(e.Booking, cancellationToken);
    }

    private async Task ProcessBookingPaymentAsync(Booking booking, CancellationToken cancellationToken)
    {
        var result = await (Random.Shared.Next(10) switch
        {
            > 4 => booking.Pay(),
            _ => booking.Cancel(CancellationReason.PaymentFailed)
        }).ThenAsync(_ => bookingRepository.UpdateAsync(booking, cancellationToken));

        if (result.IsError)
        {
            throw new ApplicationException(
                $"Failed to process payment for booking {booking.Id}: {result.FirstError.Description}");
        }
    }
}