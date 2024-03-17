using Eventify.Application.Account.Commands.Login;
using Eventify.Application.Common.Abstractions.Auth;
using Eventify.Contracts.Account.Responses;

namespace Eventify.Application.UnitTests.Account.Commands.Login;

[TestSubject(typeof(LoginCommandHandler))]
public sealed class LoginCommandHandlerTests
{
    private readonly IAuthService authService = A.Fake<IAuthService>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly LoginCommand command = new()
    {
        Email = "john@doe.com",
        Password = "john1234"
    };

    private readonly LoginCommandHandler sut;

    public LoginCommandHandlerTests()
    {
        sut = new LoginCommandHandler(authService);
    }

    [Fact]
    public async Task Handle_WhenAuthenticationIsSuccessful_ShouldReturnAuthResponse()
    {
        // Arrange
        var authResult = new AuthResult
        {
            AccessToken = "access-token",
            RefreshToken = "refresh-token",
            ExpiresIn = 3600
        };

        A.CallTo(() => authService.SignInAsync(command.Email, command.Password))
            .Returns(authResult);
        
        // Act
        var result = await sut.Handle(command, cancellationToken);
        
        // Assert
        result.Should().BeValue(authResult.Adapt<AuthResponse>());
    }
    
    [Fact]
    public async Task Handle_WhenAuthenticationIsUnsuccessful_ShouldReturnError()
    {
        // Arrange
        var error = Error.Unauthorized();
        
        A.CallTo(() => authService.SignInAsync(command.Email, command.Password))
            .Returns(error);
        
        // Act
        var result = await sut.Handle(command, cancellationToken);
        
        // Assert
        result.Should().BeError(error);
    }
}