using Amazon.AspNetCore.Identity.Cognito;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Eventify.Application.Common.Abstractions.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Eventify.Infrastructure.Common.Identity;

internal sealed class IdentityService(
    CognitoUserPool userPool,
    UserManager<CognitoUser> userManager,
    ILogger<IdentityService> logger
) : IIdentityService
{
    public async Task<ErrorOr<string>> CreateUserAsync(string email, string password)
    {
        if (await userManager.FindByEmailAsync(email) is not null)
        {
            return IdentityErrors.EmailAlreadyExists;
        }

        var user = userPool.GetUser(userID: email);
        user.Attributes.Add(CognitoAttribute.Email.AttributeName, email);

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Failed to create user: {result.Errors.First()}");
        }

        // Only to get the Cognito generated user ID
        user = await userManager.FindByEmailAsync(email);
        return user!.UserID;
    }

    public async Task AddUserToRoleAsync(string userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return;
        }

        try
        {
            await userManager.AddToRoleAsync(user, role);
        }
        catch (Exception e) when (e is ResourceNotFoundException)
        {
            logger.LogWarning("Role {Role} not found", role);
            throw;
        }
    }

    public async Task<bool> IsUserInRoleAsync(string userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId);
        return user is not null && await userManager.IsInRoleAsync(user, role);
    }
}