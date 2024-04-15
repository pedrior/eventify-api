using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Domain.Bookings;
using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.Events;
using Eventify.Domain.Tickets.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Bookings.Events;

internal sealed class BookingCancelledHandler(
    ITicketRepository ticketRepository,
    ILogger<BookingCancelledHandler> logger
) : IDomainEventHandler<BookingCancelled>
{
    public async Task Handle(BookingCancelled e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {Event} domain event", nameof(BookingCancelled));

        if (e.Reason != CancellationReason.PaymentFailed)
        {
            await ProcessTicketSaleCancellationAsync(e.Booking, cancellationToken);
        }
    }

    private async Task ProcessTicketSaleCancellationAsync(Booking booking, CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetAsync(booking.TicketId, cancellationToken);
        if (ticket is null)
        {
            throw new ApplicationException($"Ticket {booking.TicketId} not found");
        }

        await ticket.CancelSale()
            .ThrowIfFailure()
            .ThenAsync(() => ticketRepository.UpdateAsync(ticket, cancellationToken));
    }
}