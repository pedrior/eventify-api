using Eventify.Application.Tokens.Commands.RefreshToken;
using Eventify.Contracts.Tokens.Requests;

namespace Eventify.Presentation.Controllers;

[ApiVersion(1)]
public sealed class TokensController : ApiController
{
    [HttpPost("refresh"), AllowAnonymous]
    public Task<IActionResult> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        return SendAsync(request.Adapt<RefreshTokenCommand>(), cancellationToken)
            .ToResponseAsync(Ok, HttpContext);
    }
}