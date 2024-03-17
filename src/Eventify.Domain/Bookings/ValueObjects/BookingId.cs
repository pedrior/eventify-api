using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Bookings.ValueObjects;

public sealed class BookingId(Guid value) : ValueObject
{
    public Guid Value { get; } = value;

    public static implicit operator BookingId(Guid value) => new(value);    
    public static BookingId New() => new(Guid.NewGuid());
    
    public override string ToString() => Value.ToString();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}