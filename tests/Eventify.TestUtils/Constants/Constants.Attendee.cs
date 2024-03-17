using Eventify.Domain.Attendees.ValueObjects;

namespace Eventify.TestUtils.Constants;

public static partial class Constants
{
    public static class Attendee
    {
        public static readonly AttendeeId AttendeeId = AttendeeId.New();

        public static readonly AttendeeDetails Details = new(
            givenName: "John",
            familyName: "Doe",
            birthDate: new DateOnly(2000, 9, 5));

        public static readonly AttendeeContact Contact = new(
            email: "john@doe.com",
            phoneNumber: "+5581999999999");
    }
}