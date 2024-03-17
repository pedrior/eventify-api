using Eventify.Application.Common.Abstractions.Security;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Tickets.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.Common.Security.Requirements;

internal sealed class TicketOwnerRequirementHandler(
    IUser user,
    ITicketRepository ticketRepository,
    IProducerRepository producerRepository,
    IEventRepository eventRepository
) : IRequirementHandler<TicketOwnerRequirement>
{
    public async Task<AuthorizationResult> HandleAsync(TicketOwnerRequirement requirement,
        CancellationToken cancellationToken = default)
    {
        if (!await ticketRepository.ExistsAsync(requirement.TicketId, cancellationToken))
        {
            // Let request handlers handle non-existing tickets
            return AuthorizationResult.Success();
        }

        var eventId = await ticketRepository.GetEventIdAsync(requirement.TicketId, cancellationToken);
        if (eventId is null)
        {
            return AuthorizationResult.Failure("User failed to meet the ticket owner requirement.");
        }

        var producerId = await producerRepository.GetIdByUserAsync(user.Id, cancellationToken);
        var isOwner = producerId is not null && await eventRepository.IsOwnerAsync(eventId, producerId,
            cancellationToken);

        return isOwner
            ? AuthorizationResult.Success()
            : AuthorizationResult.Failure("User failed to meet the ticket owner requirement.");
    }
}