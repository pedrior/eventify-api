namespace Eventify.Contracts.Events.Requests;

public sealed record UpdateEventPeriodRequest
{
    public DateTimeOffset Start { get; init; }
    
    public DateTimeOffset End { get; init; }
}