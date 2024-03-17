using Eventify.Application.Common.Abstractions.Data;

namespace Eventify.Application.Account.Commands.Register;

public sealed record RegisterCommand : ICommand<Created>, ITransactional
{
    public required string Email { get; init; }

    public required string Password { get; init; }

    public required string GivenName { get; init; }

    public required string FamilyName { get; init; }

    public string? PhoneNumber { get; init; }

    public DateOnly? BirthDate { get; init; }
}