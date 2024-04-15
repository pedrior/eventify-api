using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Application.Bookings.Common.Errors;

internal static class BookingErrors
{
    public static Error NotFound(BookingId bookingId) => Error.NotFound(
        "Booking not found",
        code: "booking.not_found",
        details: new Dictionary<string, object?> { ["booking_id"] = bookingId.Value });

    public static Error TicketNotFound(TicketId ticketId) => Error.NotFound(
        "The booking ticket does not exist",
        code: "booking.ticket_not_found",
        details: new Dictionary<string, object?> { ["ticket_id"] = ticketId.Value });
}