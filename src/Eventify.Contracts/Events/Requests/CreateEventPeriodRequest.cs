namespace Eventify.Contracts.Events.Requests;

public sealed record CreateEventPeriodRequest
{
    public DateTimeOffset Start { get; init; }

    public DateTimeOffset End { get; init; }
}