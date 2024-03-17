namespace Eventify.Application.Common.Abstractions.Auth;

public static class AuthErrors
{
    public static readonly Error InvalidCredentials = Error.Unauthorized(
        code: "invalid_credentials",
        description: "Invalid email or password.");

    public static readonly Error UserNotConfirmed = Error.Unauthorized(
        code: "user_not_confirmed",
        description: "User is not confirmed.");

    public static readonly Error InvalidRefreshToken = Error.Unauthorized(
        code: "invalid_refresh_token",
        description: "Invalid refresh token.");
}