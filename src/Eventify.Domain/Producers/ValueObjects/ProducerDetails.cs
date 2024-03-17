using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Producers.ValueObjects;

public sealed class ProducerDetails(string name, string? bio = null) : ValueObject
{
    public string Name { get; } = name;

    public string? Bio { get; } = bio;

    public override string ToString() => Name;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Bio;
    }
}