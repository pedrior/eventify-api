namespace Eventify.Application.Common.Abstractions.Auth;

public static class AuthErrors
{
    public static readonly Error InvalidCredentials = Error.Unauthorized(
        "Invalid email or password.",
        code: "invalid_credentials");

    public static readonly Error UserNotConfirmed = Error.Unauthorized(
        "User is not confirmed.",
        code: "user_not_confirmed");

    public static readonly Error InvalidRefreshToken = Error.Unauthorized(
        "Invalid refresh token.",
        code: "invalid_refresh_token");
}