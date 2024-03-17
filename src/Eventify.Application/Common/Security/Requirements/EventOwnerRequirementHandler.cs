using Eventify.Application.Common.Abstractions.Security;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.Common.Security.Requirements;

internal sealed class EventOwnerRequirementHandler(
    IUser user,
    IProducerRepository producerRepository,
    IEventRepository eventRepository
) : IRequirementHandler<EventOwnerRequirement>
{
    public async Task<AuthorizationResult> HandleAsync(EventOwnerRequirement requirement,
        CancellationToken cancellationToken = default)
    {
        if (!await eventRepository.ExistsAsync(requirement.EventId, cancellationToken))
        {
            // Let request handlers handle non-existing events
            return AuthorizationResult.Success();
        }

        var producerId = await producerRepository.GetIdByUserAsync(user.Id, cancellationToken);
        if (producerId is null)
        {
            throw new ApplicationException($"Producer of user {user.Id} not found");
        }
        
        return  await eventRepository.IsOwnerAsync(requirement.EventId, producerId, cancellationToken)
            ? AuthorizationResult.Success()
            : AuthorizationResult.Failure("User failed to meet the event owner requirement.");
    }
}