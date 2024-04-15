using Eventify.Application.Common.Abstractions.Persistence;
using Eventify.Contracts.Tokens.Responses;

namespace Eventify.Application.Tokens.Commands.RefreshToken;

public sealed record RefreshTokenCommand : ICommand<RefreshTokenResponse>, ITransactional
{
    public required string UserId { get; init; }

    public required string RefreshToken { get; init; }
}