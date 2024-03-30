namespace Eventify.Application.Common.Abstractions.Identity;

public interface IIdentityService
{
    Task<Result<string>> CreateUserAsync(string email, string password);

    Task AddUserToRoleAsync(string userId, string role);

    Task<bool> IsUserInRoleAsync(string userId, string role);
}