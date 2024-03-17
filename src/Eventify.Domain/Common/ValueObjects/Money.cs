namespace Eventify.Domain.Common.ValueObjects;

public sealed class Money : ValueObject
{
    public static readonly Money Zero = new(0m);

    public Money(decimal value)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(value);

        Value = value;
    }

    public decimal Value { get; }

    public static Money operator +(Money left, Money right) => new(left.Value + right.Value);

    public static Money operator -(Money left, Money right) => new(left.Value - right.Value);

    public static Money operator *(Money money, Quantity quantity) => new(money.Value * quantity.Value);

    public override string ToString() => Value.ToString("C");

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}