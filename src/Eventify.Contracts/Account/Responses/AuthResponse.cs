namespace Eventify.Contracts.Account.Responses;

public sealed record AuthResponse
{
    public string AccessToken { get; init; } = null!;
    
    public string RefreshToken { get; init; } = null!;
    
    public long ExpiresIn { get; init; }
}