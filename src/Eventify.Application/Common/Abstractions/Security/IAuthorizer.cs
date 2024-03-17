namespace Eventify.Application.Common.Abstractions.Security;

internal interface IAuthorizer<in TRequest> where TRequest : IBaseRequest
{
    IEnumerable<IRequirement> GetRequirements(TRequest request);
}