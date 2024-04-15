using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Application.Tickets.Common.Errors;

internal static class TicketErrors
{
    public static Error NotFound(TicketId ticketId) => Error.NotFound(
        "Ticket not found",
        code: "ticket.not_found",
        details: new Dictionary<string, object?> { ["ticket_id"] = ticketId.Value });
    
    public static Error EventNotFound(EventId eventId) => Error.NotFound(
        "Event not found",
        code: "ticket.event_not_found",
        details: new Dictionary<string, object?> { ["event_id"] = eventId.Value });
}