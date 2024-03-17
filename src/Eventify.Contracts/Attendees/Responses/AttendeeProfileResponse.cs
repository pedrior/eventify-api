namespace Eventify.Contracts.Attendees.Responses;

public sealed record AttendeeProfileResponse
{
    public string GivenName { get; init; } = null!;

    public string FamilyName { get; init; } = null!;

    public string Email { get; init; } = null!;

    public string? PhoneNumber { get; init; }

    public DateOnly? BirthDate { get; init; }

    public Uri? PictureUrl { get; init; }

    public DateTimeOffset CreatedAt { get; init; }

    public DateTimeOffset? UpdatedAt { get; init; }
}