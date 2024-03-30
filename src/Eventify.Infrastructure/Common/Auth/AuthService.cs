using System.Security.Cryptography;
using System.Text;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Extensions.CognitoAuthentication;
using Eventify.Application.Common.Abstractions.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Eventify.Infrastructure.Common.Auth;

internal sealed class AuthService(
    UserManager<CognitoUser> userManager,
    SignInManager<CognitoUser> signInManager,
    IAmazonCognitoIdentityProvider identityProviderClient,
    IConfiguration configuration
) : IAuthService
{
    public async Task<Result<AuthResult>> SignInAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return AuthErrors.InvalidCredentials;
        }

        SignInResult result;
        try
        {
            result = await signInManager.PasswordSignInAsync(
                user,
                password,
                isPersistent: false,
                lockoutOnFailure: false);
        }
        catch (Exception e) when (e is UserNotConfirmedException)
        {
            return AuthErrors.UserNotConfirmed;
        }

        if (!result.Succeeded)
        {
            return AuthErrors.InvalidCredentials;
        }

        return new AuthResult
        {
            AccessToken = user.SessionTokens.AccessToken,
            RefreshToken = user.SessionTokens.RefreshToken,
            ExpiresIn = (long)Math.Ceiling(user.SessionTokens.ExpirationTime.Subtract(DateTime.UtcNow).TotalSeconds)
        };
    }

    public async Task<Result<AuthResult>> RefreshAsync(string userId, string refreshToken)
    {
        var clientId = configuration["AWS:UserPoolClientId"]!;
        var clientSecret = configuration["AWS:UserPoolClientSecret"]!;

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(clientSecret));
        var secretHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userId + clientId));
        var base64SecretHash = Convert.ToBase64String(secretHash);

        var request = new InitiateAuthRequest
        {
            ClientId = clientId,
            AuthFlow = AuthFlowType.REFRESH_TOKEN_AUTH,
            AuthParameters = new Dictionary<string, string>
            {
                { "SECRET_HASH", base64SecretHash },
                { "REFRESH_TOKEN", refreshToken }
            }
        };

        try
        {
            var response = await identityProviderClient.InitiateAuthAsync(request);
            return new AuthResult
            {
                AccessToken = response.AuthenticationResult.AccessToken,
                RefreshToken = refreshToken,
                ExpiresIn = response.AuthenticationResult.ExpiresIn
            };
        }
        catch (Exception e) when (e is NotAuthorizedException)
        {
            return AuthErrors.InvalidRefreshToken;
        }
    }
}