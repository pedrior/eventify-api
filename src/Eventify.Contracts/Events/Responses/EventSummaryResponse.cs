namespace Eventify.Contracts.Events.Responses;

public sealed record EventSummaryResponse
{
    public Guid Id { get; init; }

    public string Slug { get; init; } = null!;

    public string Name { get; init; } = null!;

    public string Category { get; init; } = null!;

    public string Language { get; init; } = null!;

    public string? PosterUrl { get; init; }

    public string ProducerName { get; init; } = null!;

    public string? ProducerPictureUrl { get; init; } = null!;

    public EventPeriodResponse Period { get; init; } = null!;

    public EventLocationResponse Location { get; init; } = null!;

    public DateTimeOffset? PublishedAt { get; init; }
}