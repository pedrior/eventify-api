using Eventify.Application.Common.Abstractions.Auth;
using Eventify.Contracts.Account.Responses;

namespace Eventify.Application.Account.Commands.Login;

internal sealed class LoginCommandHandler(IAuthService authService)
    : ICommandHandler<LoginCommand, AuthResponse>
{
    public async Task<ErrorOr<AuthResponse>> Handle(LoginCommand command,
        CancellationToken cancellationToken)
    {
        var result = await authService.SignInAsync(command.Email, command.Password);
        return result.Then(authResult => authResult.Adapt<AuthResponse>());
    }
}