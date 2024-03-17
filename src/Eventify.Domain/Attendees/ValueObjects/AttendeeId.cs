using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Attendees.ValueObjects;

public sealed class AttendeeId(Guid value) : ValueObject
{
    public Guid Value { get; } = value;
    
    public static AttendeeId New() => new(Guid.NewGuid());
    
    public override string ToString() => Value.ToString();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}