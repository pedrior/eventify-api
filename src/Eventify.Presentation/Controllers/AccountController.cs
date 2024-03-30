using Eventify.Application.Account.Commands.Login;
using Eventify.Application.Account.Commands.Register;
using Eventify.Contracts.Account.Requests;

namespace Eventify.Presentation.Controllers;

[ApiVersion(1)]
public sealed class AccountController : ApiController
{
    [HttpPost("login"), AllowAnonymous]
    public Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<LoginCommand>(), cancellationToken)
            .Ok(HttpContext);
    }

    [HttpPost("register"), AllowAnonymous]
    public Task<IActionResult> Register(RegisterRequest request, CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<RegisterCommand>(), cancellationToken)
            .Created("", HttpContext);
    }
}