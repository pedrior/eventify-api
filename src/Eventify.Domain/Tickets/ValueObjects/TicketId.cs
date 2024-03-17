using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Tickets.ValueObjects;

public sealed class TicketId(Guid value) : ValueObject
{
    public Guid Value { get; } = value;

    public static implicit operator TicketId(Guid value) => new(value);
    
    public static TicketId New() => new(Guid.NewGuid());
    
    public override string ToString() => Value.ToString();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}