using Eventify.Domain.Bookings.ValueObjects;

namespace Eventify.Domain.Attendees.Errors;

internal static class AttendeeErrors
{
    public static Error BookingAlreadyAdded(BookingId bookingId) => Error.Conflict(
        "The attendee already has the booking",
        code: "attendee.booking_already_added",
        metadata: new() { ["booking_id"] = bookingId.Value });

    public static Error BookingNotFound(BookingId bookingId) => Error.NotFound(
        "The attendee does not have the booking",
        code: "attendee.booking_not_found",
        metadata: new() { ["booking_id"] = bookingId.Value });

    public static Error BookingAlreadyCancelled(BookingId bookingId) => Error.Conflict(
        "The booking is already cancelled for the attendee",
        code: "attendee.booking_already_cancelled",
        metadata: new() { ["booking_id"] = bookingId.Value });
}