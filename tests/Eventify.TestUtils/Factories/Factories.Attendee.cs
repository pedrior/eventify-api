using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Users.ValueObjects;

namespace Eventify.TestUtils.Factories;

public static partial class Factories
{
    public static class Attendee
    {
        public static Domain.Attendees.Attendee CreateAttendee(
            AttendeeId? attendeeId = null,
            UserId? userId = null,
            AttendeeDetails? details = null,
            AttendeeContact? contact = null,
            Uri? pictureUrl = null)
        {
            return Domain.Attendees.Attendee.Create(
                attendeeId ?? Constants.Constants.Attendee.AttendeeId,
                userId ?? Constants.Constants.UserId,
                details ?? Constants.Constants.Attendee.Details,
                contact ?? Constants.Constants.Attendee.Contact,
                pictureUrl);
        }
    }
}