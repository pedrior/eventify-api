namespace Eventify.Contracts.Tokens.Requests;

public sealed record RefreshTokenRequest
{
    public string UserId { get; init; } = null!;

    public string RefreshToken { get; init; } = null!;
}