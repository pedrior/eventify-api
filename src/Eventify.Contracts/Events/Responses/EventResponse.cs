namespace Eventify.Contracts.Events.Responses;

public sealed record EventResponse
{
    public Guid Id { get; init; }

    public string Slug { get; init; } = null!;

    public string Name { get; init; } = null!;

    public string State { get; init; } = null!;

    public string? Description { get; init; }

    public string Category { get; init; } = null!;

    public string Language { get; init; } = null!;

    public string? PosterUrl { get; init; }

    public int BookingCount { get; init; }

    public EventPeriodResponse Period { get; init; } = null!;

    public EventLocationResponse Location { get; init; } = null!;

    public EventProducerResponse Producer { get; init; } = null!;

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset? UpdatedAt { get; init; }

    public DateTimeOffset? PublishedAt { get; init; }

    public IEnumerable<EventTicketResponse> Tickets { get; init; } = [];
}