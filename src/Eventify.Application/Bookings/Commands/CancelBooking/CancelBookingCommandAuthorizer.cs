using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Bookings.Commands.CancelBooking;

internal sealed class CancelBookingCommandAuthorizer : IAuthorizer<CancelBookingCommand>
{
    public IEnumerable<IRequirement> GetRequirements(CancelBookingCommand command)
    {
        yield return new RoleRequirement(Roles.Attendee);
        yield return new BookingOwnerRequirement(command.BookingId);
    }
}