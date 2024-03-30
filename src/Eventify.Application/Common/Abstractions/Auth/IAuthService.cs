namespace Eventify.Application.Common.Abstractions.Auth;

public interface IAuthService
{
    Task<Result<AuthResult>> SignInAsync(string email, string password);
    
    Task<Result<AuthResult>> RefreshAsync(string userId, string refreshToken);
}