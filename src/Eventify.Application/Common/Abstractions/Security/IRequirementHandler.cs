namespace Eventify.Application.Common.Abstractions.Security;

internal interface IRequirementHandler
{
    Task<AuthorizationResult> HandleAsync(object requirement, CancellationToken cancellationToken = default);
}