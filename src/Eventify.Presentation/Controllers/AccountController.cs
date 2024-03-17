using Eventify.Application.Account.Commands.Login;
using Eventify.Application.Account.Commands.Register;
using Eventify.Contracts.Account.Requests;

namespace Eventify.Presentation.Controllers;

[ApiVersion(1)]
public sealed class AccountController : ApiController
{
    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<LoginCommand>(), cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }
    
    [HttpPost("register"), AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<RegisterCommand>(), cancellationToken);
        return result.Match(onValue: _ => Created(), onError: Problem);
    }
}