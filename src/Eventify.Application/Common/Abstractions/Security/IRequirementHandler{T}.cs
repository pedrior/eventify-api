namespace Eventify.Application.Common.Abstractions.Security;

internal interface IRequirementHandler<in TRequirement> 
    : IRequirementHandler where TRequirement : IRequirement
{
    Task<AuthorizationResult> HandleAsync(TRequirement requirement,
        CancellationToken cancellationToken = default);
    
    Task<AuthorizationResult> IRequirementHandler.HandleAsync(object requirement, 
        CancellationToken cancellationToken) => HandleAsync((TRequirement)requirement, cancellationToken);
}