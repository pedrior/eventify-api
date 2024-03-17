using System.Security.Claims;
using Eventify.Domain.Users;
using Eventify.Domain.Users.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Eventify.Infrastructure.Users;

internal sealed class UserContext : IUser
{
    public UserContext(IHttpContextAccessor httpContextAccessor)
    {
        var user = httpContextAccessor.HttpContext?.User;
        if (user is not { Identity.IsAuthenticated: true })
        {
            return;
        }

        var sub = user.FindFirstValue(JwtRegisteredClaimNames.Sub);
        Id = string.IsNullOrWhiteSpace(sub) ? UserId.Empty : new UserId(sub);
    }

    public UserId Id { get; } = UserId.Empty;
}