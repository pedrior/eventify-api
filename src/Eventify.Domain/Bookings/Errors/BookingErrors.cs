using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Domain.Bookings.Errors;

internal static class BookingErrors
{
    public static Error InvalidStateOperation(BookingState state) => Error.Conflict(
        "Invalid operation for the current booking state",
        code: "booking.invalid_state_operation",
        details: new Dictionary<string, object?> { ["state"] = state.Name });

    public static Error RequiredPublishedEvent(EventId eventId) => Error.Failure(
        "Cannot place a booking for an unpublished event",
        code: "booking.required_published_event",
        details: new Dictionary<string, object?> { ["event_id"] = eventId.Value });

    public static Error UnavailableTicket(TicketId ticketId) => Error.Failure(
        "Cannot place a booking for an unavailable ticket",
        code: "booking.unavailable_ticket",
        details: new Dictionary<string, object?> { ["ticket_id"] = ticketId.Value });

    public static Error MultipleActiveBooking(BookingId bookingId, TicketId ticketId) => Error.Conflict(
        "Cannot place a booking for a ticket for which the attendee already has an active booking",
        code: "booking.multiple_active_booking",
        details: new Dictionary<string, object?>
        {
            ["booking_id"] = bookingId.Value,
            ["ticket_id"] = ticketId.Value
        });
}