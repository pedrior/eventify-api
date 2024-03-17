using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.Enums;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Domain.Events.Errors;

internal static class EventErrors
{
    public static readonly Error MustBeInFuture = Error.Conflict(
        code: "event.must_be_in_future",
        description: "The event must be in the future");

    public static readonly Error MustHaveTicket = Error.Conflict(
        code: "event.must_have_ticket",
        description: "The event must have at least one ticket");

    public static readonly Error TicketSalePeriodExceedsEventPeriod = Error.Conflict(
        code: "event.ticket_sale_period_exceeds_event_period",
        description: "The ticket sale period exceeds the event period");

    public static readonly Error CannotModifyFinishedEvent = Error.Conflict(
        code: "event.cannot_modify_finished_event",
        description: "Cannot modify a finished event");
    
    public static Error InvalidOperation(EventState state) => Error.Conflict(
        code: "event.invalid_operation",
        description: "Invalid operation for the current event state",
        metadata: new() { ["state"] = state.Name });

    public static Error SlugAlreadyExists(Slug slug) => Error.Conflict(
        code: "event.slug_already_exists",
        description: "The slug is already associated with another event",
        metadata: new() { ["slug"] = slug.Value });

    public static Error TicketAlreadyAdded(TicketId ticketId) => Error.Conflict(
        code: "event.ticket_already_added",
        description: "The event already has the ticket",
        metadata: new() { ["ticket_id"] = ticketId.Value });
    
    public static Error TicketNotFound(TicketId ticketId) => Error.NotFound(
        code: "event.ticket_not_found",
        description: "The event does not have the ticket",
        metadata: new() { ["ticket_id"] = ticketId.Value });
    
    public static Error TicketLimitReached(int limit) => Error.Conflict(
        code: "event.ticket_limit_reached",
        description: "The event has reached the maximum number of tickets",
        metadata: new() { ["limit"] = limit });

    public static Error BookingAlreadyAdded(BookingId ticketId) => Error.Conflict(
        code: "event.booking_already_added",
        description: "The event already has the booking",
        metadata: new() { ["booking_id"] = ticketId.Value });
}