using Eventify.Contracts.Account.Responses;

namespace Eventify.Application.Account.Commands.Login;

public sealed record LoginCommand : ICommand<AuthResponse>
{
    public required string Email { get; init; }

    public required string Password { get; init; }
}