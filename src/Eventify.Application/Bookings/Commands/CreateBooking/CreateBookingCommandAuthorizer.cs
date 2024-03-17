using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Bookings.Commands.CreateBooking;

internal sealed class CreateBookingCommandAuthorizer : IAuthorizer<CreateBookingCommand>
{
    public IEnumerable<IRequirement> GetRequirements(CreateBookingCommand request)
    {
        yield return new RoleRequirement(Roles.Attendee);
    }
}