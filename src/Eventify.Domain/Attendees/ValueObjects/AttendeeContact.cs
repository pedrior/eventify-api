using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Attendees.ValueObjects;

public sealed class AttendeeContact(string email, string? phoneNumber = null) : ValueObject
{
    public string Email { get; } = email;

    public string? PhoneNumber { get; } = phoneNumber;

    public override string ToString() =>
        PhoneNumber is not null
            ? $"{Email}, {PhoneNumber}"
            : Email;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Email;
        yield return PhoneNumber;
    }
}