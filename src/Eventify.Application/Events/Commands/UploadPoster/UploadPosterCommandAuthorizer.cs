using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Events.Commands.UploadPoster;

internal sealed class UploadPosterCommandAuthorizer : IAuthorizer<UploadPosterCommand>
{
    public IEnumerable<IRequirement> GetRequirements(UploadPosterCommand command)
    {
        yield return new RoleRequirement(Roles.Producer);
        yield return new EventOwnerRequirement(command.EventId);
    }
}