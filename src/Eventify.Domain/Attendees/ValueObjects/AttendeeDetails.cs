using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Attendees.ValueObjects;

public sealed class AttendeeDetails(string givenName, string familyName, DateOnly? birthDate) : ValueObject
{
    public string GivenName { get; } = givenName;

    public string FamilyName { get; } = familyName;

    public DateOnly? BirthDate { get; } = birthDate;

    public override string ToString() => $"{GivenName} {FamilyName}, {BirthDate:yyyy-MM-dd}";

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return GivenName;
        yield return FamilyName;
        yield return BirthDate;
    }
}