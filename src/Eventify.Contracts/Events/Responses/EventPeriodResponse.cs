namespace Eventify.Contracts.Events.Responses;

public sealed record EventPeriodResponse
{
    public DateTimeOffset Start { get; init; }

    public DateTimeOffset End { get; init; }
}