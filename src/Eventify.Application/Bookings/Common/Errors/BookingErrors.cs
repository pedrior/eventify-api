using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Application.Bookings.Common.Errors;

internal static class BookingErrors
{
    public static Error NotFound(BookingId bookingId) => Error.NotFound(
        code: "booking.not_found",
        description: "Booking not found",
        metadata: new() { ["booking_id"] = bookingId.Value });

    public static Error TicketNotFound(TicketId ticketId) => Error.NotFound(
        code: "booking.ticket_not_found",
        description: "The booking ticket does not exist",
        metadata: new() { ["ticket_id"] = ticketId.Value });
}