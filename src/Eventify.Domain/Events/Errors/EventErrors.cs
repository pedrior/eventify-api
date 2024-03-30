using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.Enums;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Domain.Events.Errors;

internal static class EventErrors
{
    public static readonly Error MustBeInFuture = Error.Conflict(
        "The event must be in the future",
        code: "event.must_be_in_future");

    public static readonly Error MustHaveTicket = Error.Conflict(
        "The event must have at least one ticket",
        code: "event.must_have_ticket");

    public static readonly Error TicketSalePeriodExceedsEventPeriod = Error.Conflict(
        "The ticket sale period exceeds the event period",
        code: "event.ticket_sale_period_exceeds_event_period");

    public static readonly Error CannotModifyFinishedEvent = Error.Conflict(
        "Cannot modify a finished event",
        code: "event.cannot_modify_finished_event");

    public static Error InvalidOperation(EventState state) => Error.Conflict(
        "Invalid operation for the current event state",
        code: "event.invalid_operation",
        metadata: new() { ["state"] = state.Name });

    public static Error SlugAlreadyExists(Slug slug) => Error.Conflict(
        "The slug is already associated with another event",
        code: "event.slug_already_exists",
        metadata: new() { ["slug"] = slug.Value });

    public static Error TicketAlreadyAdded(TicketId ticketId) => Error.Conflict(
        "The event already has the ticket",
        code: "event.ticket_already_added",
        metadata: new() { ["ticket_id"] = ticketId.Value });

    public static Error TicketNotFound(TicketId ticketId) => Error.NotFound(
        "The event does not have the ticket",
        code: "event.ticket_not_found",
        metadata: new() { ["ticket_id"] = ticketId.Value });

    public static Error TicketLimitReached(int limit) => Error.Conflict(
        "The event has reached the maximum number of tickets",
        code: "event.ticket_limit_reached",
        metadata: new() { ["limit"] = limit });

    public static Error BookingAlreadyAdded(BookingId ticketId) => Error.Conflict(
        "The event already has the booking",
        code: "event.booking_already_added",
        metadata: new() { ["booking_id"] = ticketId.Value });
}