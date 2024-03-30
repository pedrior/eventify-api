using Eventify.Application.Common.Abstractions.Auth;
using Eventify.Contracts.Tokens.Responses;

namespace Eventify.Application.Tokens.Commands.RefreshToken;

internal sealed class RefreshTokenCommandHandler(IAuthService authService)
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResponse>
{
    public async Task<Result<RefreshTokenResponse>> Handle(RefreshTokenCommand command,
        CancellationToken cancellationToken)
    {
        var result = await authService.RefreshAsync(command.UserId, command.RefreshToken);
        return result.Then(auth => auth.Adapt<RefreshTokenResponse>());
    }
}