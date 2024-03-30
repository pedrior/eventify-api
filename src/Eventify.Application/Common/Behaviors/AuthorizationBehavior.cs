using Eventify.Application.Common.Abstractions.Security;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Common.Behaviors;

internal sealed class AuthorizationBehavior<TRequest, TResponse>(
    IServiceProvider serviceProvider,
    ILogger<AuthorizationBehavior<TRequest, TResponse>> logger,
    IAuthorizer<TRequest>? authorizer = null
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IBaseRequest
    where TResponse : IResult
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (authorizer is null)
        {
            return await next();
        }

        foreach (var requirement in authorizer.GetRequirements(request))
        {
            var handler = ResolveRequirementHandler(requirement.GetType(), serviceProvider);
            var result = await handler.HandleAsync(requirement, cancellationToken);

            if (result.Succeeded)
            {
                continue;
            }

            logger.LogInformation("Request {RequestName} was forbidden - {ForbiddenReason}",
                request.GetType().Name, result.Reason);

            return (TResponse)(dynamic)Error.Forbidden("You are not authorized to perform this action.");
        }

        return await next();
    }

    private static IRequirementHandler ResolveRequirementHandler(Type requirementType,
        IServiceProvider serviceProvider)
    {
        var handlerType = typeof(IRequirementHandler<>).MakeGenericType(requirementType);
        return serviceProvider.GetRequiredService(handlerType) as IRequirementHandler
               ?? throw new InvalidOperationException($"No requirement handler found for '{requirementType.Name}'");
    }
}