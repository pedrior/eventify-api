using Microsoft.AspNetCore.RateLimiting;

namespace Eventify.Presentation.Common.Controllers;

[Authorize]
[ApiController]
[EnableRateLimiting("default")]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiController : ControllerBase
{
    private ISender? sender;

    private ISender Sender => sender ??= HttpContext.RequestServices.GetService<ISender>()!;

    protected Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request,
        CancellationToken cancellationToken = default)
    {
        return Sender.Send(request, cancellationToken);
    }
}