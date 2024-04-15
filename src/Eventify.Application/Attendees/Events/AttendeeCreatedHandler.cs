using Eventify.Application.Common.Abstractions.Identity;
using Eventify.Application.Common.Security;
using Eventify.Domain.Attendees.Events;
using Eventify.Domain.Users.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Attendees.Events;

internal sealed class AttendeeCreatedHandler(
    IIdentityService identityService,
    ILogger<AttendeeCreatedHandler> logger
) : IDomainEventHandler<AttendeeCreated>
{
    public Task Handle(AttendeeCreated e, CancellationToken _)
    {
        logger.LogInformation("Handling {EventName} domain event", nameof(AttendeeCreated));

        return AddUserToAttendeeRoleAsync(e.Attendee.UserId);
    }

    private async Task AddUserToAttendeeRoleAsync(UserId userId)
    {
        await identityService.AddUserToRoleAsync(userId, Roles.Attendee);

        logger.LogInformation("User '{UserId}' has been assigned to role {Role}",
            userId, Roles.Attendee);
    }
}