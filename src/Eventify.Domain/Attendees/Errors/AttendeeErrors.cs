using Eventify.Domain.Bookings.ValueObjects;

namespace Eventify.Domain.Attendees.Errors;

internal static class AttendeeErrors
{
    public static Error BookingAlreadyAdded(BookingId bookingId) => Error.Conflict(
        "The attendee already has the booking",
        code: "attendee.booking_already_added",
        details: new Dictionary<string, object?> { ["booking_id"] = bookingId.Value }.ToFrozenDictionary());

    public static Error BookingNotFound(BookingId bookingId) => Error.NotFound(
        "The attendee does not have the booking",
        code: "attendee.booking_not_found",
        details: new Dictionary<string, object?> { ["booking_id"] = bookingId.Value }.ToFrozenDictionary());

    public static Error BookingAlreadyCancelled(BookingId bookingId) => Error.Conflict(
        "The booking is already cancelled for the attendee",
        code: "attendee.booking_already_cancelled",
        details: new Dictionary<string, object?> { ["booking_id"] = bookingId.Value }.ToFrozenDictionary());
}