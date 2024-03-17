namespace Eventify.Contracts.Events.Requests;

public sealed record UpdateEventSlugRequest
{
    public string Slug { get; init; } = null!;
}