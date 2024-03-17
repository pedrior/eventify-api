using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Events.ValueObjects;

public sealed class EventId(Guid value) : ValueObject
{
    public Guid Value { get; } = value;

    public static implicit operator EventId(Guid value) => new(value);
    
    public static EventId New() => new(Guid.NewGuid());
    
    public override string ToString() => Value.ToString();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}