namespace Eventify.Domain.Common.ValueObjects;

public sealed class Quantity : ValueObject
{
    public static readonly Quantity Zero = new(0);
    
    public Quantity(int value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);

        Value = value;
    }

    public int Value { get; }

    public static implicit operator int(Quantity quantity) => quantity.Value;
    
    public static implicit operator Quantity(int value) => new(value);

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}