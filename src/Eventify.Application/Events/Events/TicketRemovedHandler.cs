using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Events.Events;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;
using Eventify.Domain.Tickets.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Events.Events;

internal sealed class TicketRemovedHandler(
    IBookingRepository bookingRepository,
    ITicketRepository ticketRepository,
    ILogger<TicketRemovedHandler> logger
) : IDomainEventHandler<TicketRemoved>
{
    public async Task Handle(TicketRemoved e, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling {EventName} domain event", nameof(TicketRemoved));

        await CancelAllTicketBookingsAsync(e.Ticket.Id, cancellationToken);
        await RemoveTicketAsync(e.Ticket, cancellationToken);
    }

    private async Task CancelAllTicketBookingsAsync(TicketId ticketId, CancellationToken cancellationToken)
    {
        foreach (var booking in await bookingRepository.ListByTicketAsync(ticketId, cancellationToken))
        {
            await booking.Cancel(CancellationReason.EventCancelled)
                .ThenAsync(() => bookingRepository.UpdateAsync(booking, cancellationToken))
                .Else(errors =>
                {
                    logger.LogError("Error canceling booking {BookingId} for ticket {TicketId}: {@Error}",
                        booking.Id, ticketId, errors[0]);
                });
        }
    }

    private Task RemoveTicketAsync(Ticket ticket, CancellationToken cancellationToken) =>
        ticketRepository.RemoveAsync(ticket, cancellationToken);
}