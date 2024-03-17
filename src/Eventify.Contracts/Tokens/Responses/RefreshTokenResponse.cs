namespace Eventify.Contracts.Tokens.Responses;

public sealed record RefreshTokenResponse
{
    public string AccessToken { get; init; } = null!;
    
    public string RefreshToken { get; init; } = null!;
    
    public long ExpiresIn { get; init; }
}