using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.Events;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Tickets.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Bookings.Events;

internal sealed class BookingPaidHandler(
    IBookingRepository bookingRepository,
    ITicketRepository ticketRepository,
    ILogger<BookingPaidHandler> logger
) : IDomainEventHandler<BookingPaid>
{
    public async Task Handle(BookingPaid e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Event} domain event", nameof(BookingPaid));

        await ConfirmBookingAsync(e.Booking, cancellationToken);
    }

    private async Task ConfirmBookingAsync(Booking booking, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetAsync(booking.TicketId, cancellationToken);
        if (ticket is null)
        {
            throw new ApplicationException($"Ticket {booking.TicketId} not found");
        }

        await ticket.Sell()
            .Then(_ => booking.Confirm())
            .ThenAsync(() => bookingRepository.UpdateAsync(booking, cancellationToken))
            .ElseAsync(async () =>
            {
                logger.LogInformation("Cancelling booking {BookingId} due to confirmation failure", booking.Id);

                await booking.Cancel(CancellationReason.TicketUnavailable)
                    .ThrowIfFailure()
                    .ThenAsync(() => bookingRepository.UpdateAsync(booking, cancellationToken));
            });
    }
}