using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Users.ValueObjects;

public sealed class UserId(string value) : ValueObject
{
    public static readonly UserId Empty = new(string.Empty);
    
    public string Value { get; } = value;

    public static implicit operator string(UserId userId) => userId.Value;
    
    public override string ToString() => Value;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}