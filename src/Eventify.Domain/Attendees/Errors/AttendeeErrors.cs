using Eventify.Domain.Bookings.ValueObjects;

namespace Eventify.Domain.Attendees.Errors;

internal static class AttendeeErrors
{
    public static Error BookingAlreadyAdded(BookingId bookingId) => Error.Conflict(
        code: "attendee.booking_already_added",
        description: "The attendee already has the booking",
        metadata: new() { ["booking_id"] = bookingId.Value });

    public static Error BookingNotFound(BookingId bookingId) => Error.NotFound(
        code: "attendee.booking_not_found",
        description: "The attendee does not have the booking",
        metadata: new() { ["booking_id"] = bookingId.Value });

    public static Error BookingAlreadyCancelled(BookingId bookingId) => Error.Conflict(
        code: "attendee.booking_already_cancelled",
        description: "The booking is already cancelled for the attendee",
        metadata: new() { ["booking_id"] = bookingId.Value });
}