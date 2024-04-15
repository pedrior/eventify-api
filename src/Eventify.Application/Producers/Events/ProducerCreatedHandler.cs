using Eventify.Application.Common.Abstractions.Identity;
using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Application.Common.Security;
using Eventify.Domain.Producers.Events;
using Eventify.Domain.Users.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Producers.Events;

internal sealed class ProducerCreatedHandler(
    IIdentityService identityService,
    ILogger<ProducerCreatedHandler> logger
) : IDomainEventHandler<ProducerCreated>
{
    public Task Handle(ProducerCreated e, CancellationToken _)
    {
        logger.LogInformation("Handling {EventName} domain event", nameof(ProducerCreated));

        return AddUserToProducerRoleAsync(e.Producer.UserId);
    }

    private async Task AddUserToProducerRoleAsync(UserId userId)
    {
        await identityService.AddUserToRoleAsync(userId, Roles.Producer);
        
        logger.LogInformation("User '{UserId}' has been assigned to role {Role}",
            userId, Roles.Producer);
    }
}