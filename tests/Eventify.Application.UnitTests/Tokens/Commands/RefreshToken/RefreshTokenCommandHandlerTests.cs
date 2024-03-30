using Eventify.Application.Common.Abstractions.Auth;
using Eventify.Application.Tokens.Commands.RefreshToken;
using Eventify.Contracts.Tokens.Responses;

namespace Eventify.Application.UnitTests.Tokens.Commands.RefreshToken;

[TestSubject(typeof(RefreshTokenCommandHandler))]
public sealed class RefreshTokenCommandHandlerTests
{
    private readonly IAuthService authService = A.Fake<IAuthService>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly RefreshTokenCommand command = new()
    {
        UserId = "some-id",
        RefreshToken = "some-token"
    };

    private readonly RefreshTokenCommandHandler sut;

    public RefreshTokenCommandHandlerTests()
    {
        sut = new RefreshTokenCommandHandler(authService);
    }

    [Fact]
    public async Task Handle_WhenRefreshingIsSuccessful_ShouldReturnRefreshResponse()
    {
        // Arrange
        var authResult = new AuthResult
        {
            AccessToken = "access-token",
            RefreshToken = "refresh-token",
            ExpiresIn = 3600
        };

        A.CallTo(() => authService.RefreshAsync(command.UserId, command.RefreshToken))
            .Returns(authResult);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSuccess(authResult.Adapt<RefreshTokenResponse>());
    }

    [Fact]
    public async Task Handle_WhenRefreshingIsUnsuccessful_ShouldReturnError()
    {
        // Arrange
        var error = Error.Unauthorized("Invalid refresh token.");

        A.CallTo(() => authService.RefreshAsync(command.UserId, command.RefreshToken))
            .Returns(error);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeFailure(error);
    }
}