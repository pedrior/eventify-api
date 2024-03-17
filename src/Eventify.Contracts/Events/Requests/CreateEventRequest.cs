namespace Eventify.Contracts.Events.Requests;

public sealed record CreateEventRequest
{
    public string Name { get; init; } = null!;

    public string Category { get; init; } = null!;

    public string Language { get; init; } = null!;

    public string? Description { get; init; }

    public CreateEventPeriodRequest Period { get; init; } = null!;

    public CreateEventLocationRequest Location { get; init; } = null!;
}