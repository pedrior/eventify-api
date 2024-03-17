using Microsoft.Extensions.Logging;

namespace Eventify.Application.Common.Behaviors;

internal sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : IErrorOr
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        logger.LogInformation("Handling request {RequestName}", requestName);

        var response = await next();
        if (response.IsError)
        {
            logger.LogError("Request {RequestName} failed with errors: {@Errors}", requestName, response.Errors);
        }
        else
        {
            logger.LogInformation("Request {RequestName} handled successfully", requestName);
        }

        return response;
    }
}