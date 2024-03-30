namespace Eventify.Application.Common.Abstractions.Identity;

public static class IdentityErrors
{
    public static readonly Error EmailAlreadyExists = Error.Conflict(
        "Email address already exists.",
        code: "email_already_exists");
}