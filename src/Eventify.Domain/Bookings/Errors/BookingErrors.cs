using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Domain.Bookings.Errors;

internal static class BookingErrors
{
    public static Error InvalidStateOperation(BookingState state) => Error.Conflict(
        code: "booking.invalid_state_operation",
        description: "Invalid operation for the current booking state",
        metadata: new() { ["state"] = state.Name });

    public static Error RequiredPublishedEvent(EventId eventId) => Error.Failure(
        code: "booking.required_published_event",
        description: "Cannot place a booking for an unpublished event",
        metadata: new() { ["event_id"] = eventId.Value });

    public static Error UnavailableTicket(TicketId ticketId) => Error.Failure(
        code: "booking.unavailable_ticket",
        description: "Cannot place a booking for an unavailable ticket",
        metadata: new() { ["ticket_id"] = ticketId.Value });

    public static Error MultipleActiveBooking(BookingId bookingId, TicketId ticketId) => Error.Conflict(
        code: "booking.multiple_active_booking",
        description: "Cannot place a booking for a ticket for which the attendee already has an active booking",
        metadata: new()
        {
            ["booking_id"] = bookingId.Value,
            ["ticket_id"] = ticketId.Value
        });
}