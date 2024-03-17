namespace Eventify.Application.Common.Abstractions.Identity;

public static class IdentityErrors
{
    public static readonly Error EmailAlreadyExists = Error.Conflict(
        code: "email_already_exists",
        description: "Email address already exists.");
}