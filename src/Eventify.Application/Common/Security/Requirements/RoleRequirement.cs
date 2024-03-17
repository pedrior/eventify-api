using Eventify.Application.Common.Abstractions.Security;

namespace Eventify.Application.Common.Security.Requirements;

internal sealed record RoleRequirement(params string[] Roles) : IRequirement;