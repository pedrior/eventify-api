using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Producers.ValueObjects;

public sealed class ProducerContact(string email, string phoneNumber) : ValueObject
{
    public string Email { get; } = email;

    public string PhoneNumber { get; } = phoneNumber;

    public override string ToString() => $"{Email}, {PhoneNumber}";

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Email;
        yield return PhoneNumber;
    }
}