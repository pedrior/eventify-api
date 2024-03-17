using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Application.Tickets.Common.Errors;

internal static class TicketErrors
{
    public static Error NotFound(TicketId ticketId) => Error.NotFound(
        code: "ticket.not_found",
        description: "Ticket not found",
        metadata: new()
            { ["ticket_id"] = ticketId.Value });
    
    public static Error EventNotFound(EventId eventId) => Error.NotFound(
        code: "ticket.event_not_found",
        description: "Event not found",
        metadata: new() { ["event_id"] = eventId.Value });
}