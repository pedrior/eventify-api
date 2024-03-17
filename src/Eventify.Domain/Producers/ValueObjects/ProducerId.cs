using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Producers.ValueObjects;

public sealed class ProducerId(Guid value) : ValueObject
{
    public Guid Value { get; } = value;
    
    public static ProducerId New() => new(Guid.NewGuid());
    
    public override string ToString() => Value.ToString();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}