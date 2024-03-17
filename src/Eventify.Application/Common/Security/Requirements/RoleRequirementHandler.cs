using Eventify.Application.Common.Abstractions.Identity;
using Eventify.Application.Common.Abstractions.Security;
using Eventify.Domain.Users;

namespace Eventify.Application.Common.Security.Requirements;

internal sealed class RoleRequirementHandler(IUser user, IIdentityService identityService)
    : IRequirementHandler<RoleRequirement>
{
    public async Task<AuthorizationResult> HandleAsync(RoleRequirement requirement,
        CancellationToken cancellationToken = default)
    {
        var inAnyRole = false;
        foreach (var role in requirement.Roles)
        {
            inAnyRole = await identityService.IsUserInRoleAsync(user.Id.Value, role);
            if (inAnyRole)
            {
                break;
            }
        }

        return inAnyRole
            ? AuthorizationResult.Success()
            : AuthorizationResult.Failure($"User failed to meet the role requirement. " +
                                          $"Required roles: {string.Join(", ", requirement.Roles)}.");
    }
}