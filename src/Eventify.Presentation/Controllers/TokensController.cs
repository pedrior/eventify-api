using Eventify.Application.Tokens.Commands.RefreshToken;
using Eventify.Contracts.Tokens.Requests;

namespace Eventify.Presentation.Controllers;

[ApiVersion(1)]
public sealed class TokensController : ApiController
{
    [HttpPost("refresh"), AllowAnonymous]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await SendAsync(request.Adapt<RefreshTokenCommand>(), cancellationToken);
        return result.Match(onValue: Ok, onError: Problem);
    }
}