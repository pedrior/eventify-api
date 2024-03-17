using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Events.ValueObjects;

public sealed class EventLocation(
    string name,
    string address,
    string zipCode,
    string city,
    string state,
    string country) : ValueObject
{
    public string Name { get; } = name;

    public string Address { get; } = address;

    public string ZipCode { get; } = zipCode;

    public string City { get; } = city;

    public string State { get; } = state;

    public string Country { get; } = country;

    public override string ToString() => $"{Name}, {Address}, {City}, {State}";

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Address;
        yield return ZipCode;
        yield return City;
        yield return State;
        yield return Country;
    }
}