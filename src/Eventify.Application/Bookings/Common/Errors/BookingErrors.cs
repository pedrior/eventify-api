using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Application.Bookings.Common.Errors;

internal static class BookingErrors
{
    public static Error NotFound(BookingId bookingId) => Error.NotFound(
        "Booking not found",
        code: "booking.not_found",
        metadata: new() { ["booking_id"] = bookingId.Value });

    public static Error TicketNotFound(TicketId ticketId) => Error.NotFound(
        "The booking ticket does not exist",
        code: "booking.ticket_not_found",
        metadata: new() { ["ticket_id"] = ticketId.Value });
}