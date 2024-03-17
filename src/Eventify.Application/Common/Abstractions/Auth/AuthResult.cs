namespace Eventify.Application.Common.Abstractions.Auth;

public sealed record AuthResult
{
    public required string AccessToken { get; init; }
    
    public required string RefreshToken { get; init; }
    
    public required long ExpiresIn { get; init; }
}