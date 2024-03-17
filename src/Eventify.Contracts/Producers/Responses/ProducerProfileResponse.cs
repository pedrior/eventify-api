namespace Eventify.Contracts.Producers.Responses;

public sealed record ProducerProfileResponse
{
    public string Name { get; init; } = null!;

    public string? Bio { get; init; } = null!;

    public string Email { get; init; } = null!;

    public string PhoneNumber { get; init; } = null!;

    public Uri? WebsiteUrl { get; init; }

    public Uri? PictureUrl { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset? UpdatedAt { get; init; }
}