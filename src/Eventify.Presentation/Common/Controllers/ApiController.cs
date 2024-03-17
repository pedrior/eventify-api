using Microsoft.AspNetCore.Mvc.ModelBinding;
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

    protected IActionResult Problem(List<Error> errors) =>
        errors.All(error => error.Type is ErrorType.Validation)
            ? ValidationProblem(errors)
            : Problem(errors.First());

    private ObjectResult Problem(Error error)
    {
        var statusCode = GetStatusCodeForError(error);
        var details = ProblemDetailsFactory.CreateProblemDetails(
            HttpContext, statusCode, title: error.Description);

        if (!string.IsNullOrEmpty(error.Code))
        {
            details.Extensions["code"] = error.Code;
        }

        foreach (var pair in error.Metadata ?? [])
        {
            details.Extensions[pair.Key] = pair.Value;
        }
        
        return new ObjectResult(details)
        {
            ContentTypes = { "application/problem+json" },
            StatusCode = statusCode
        };
    }

    private ActionResult ValidationProblem(IEnumerable<Error> errors)
    {
        var state = new ModelStateDictionary();
        foreach (var error in errors)
        {
            state.AddModelError(error.Code, error.Description);
        }

        return ValidationProblem(state);
    }

    private static int GetStatusCodeForError(Error error) => error switch
    {
        { Type: ErrorType.Validation } => StatusCodes.Status400BadRequest,
        { Type: ErrorType.Unauthorized } => StatusCodes.Status401Unauthorized,
        { Type: ErrorType.Forbidden } => StatusCodes.Status403Forbidden,
        { Type: ErrorType.NotFound } => StatusCodes.Status404NotFound,
        { Type: ErrorType.Conflict } => StatusCodes.Status409Conflict,
        { Type: ErrorType.Failure } => StatusCodes.Status422UnprocessableEntity,
        _ => StatusCodes.Status500InternalServerError
    };
}