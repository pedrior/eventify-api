namespace Eventify.Contracts.Events.Requests;

public sealed record UpdateEventDetailsRequest
{
    public string Name { get; init; } = null!;

    public string Category { get; init; } = null!;

    public string Language { get; init; } = null!;

    public string? Description { get; init; }
}