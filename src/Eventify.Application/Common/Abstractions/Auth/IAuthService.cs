namespace Eventify.Application.Common.Abstractions.Auth;

public interface IAuthService
{
    Task<ErrorOr<AuthResult>> SignInAsync(string email, string password);
    
    Task<ErrorOr<AuthResult>> RefreshAsync(string userId, string refreshToken);
}