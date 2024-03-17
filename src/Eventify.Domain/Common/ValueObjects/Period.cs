namespace Eventify.Domain.Common.ValueObjects;

public sealed class Period : ValueObject
{
    public Period(DateTimeOffset start, DateTimeOffset end)
    {
        if (start > end)
        {
            throw new ArgumentException("Start date must be before end date.", nameof(start));
        }

        Start = start;
        End = end;
    }
    
    public DateTimeOffset Start { get; }
    
    public DateTimeOffset End { get; }
    
    public bool IsPast => End < DateTimeOffset.Now;
    
    public override string ToString() => $"{Start} - {End}";
    
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }
}