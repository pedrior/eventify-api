namespace Eventify.Contracts.Events.Requests;

public sealed record GetEventsRequest
{
    public int Page { get; init; } = 1;

    public int Limit { get; init; } = 15;

    public string? Term { get; init; }
}