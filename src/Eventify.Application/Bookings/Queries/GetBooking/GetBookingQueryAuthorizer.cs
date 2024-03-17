using Eventify.Application.Common.Abstractions.Security;
using Eventify.Application.Common.Security;
using Eventify.Application.Common.Security.Requirements;

namespace Eventify.Application.Bookings.Queries.GetBooking;

internal sealed class GetBookingQueryAuthorizer : IAuthorizer<GetBookingQuery>
{
    public IEnumerable<IRequirement> GetRequirements(GetBookingQuery query)
    {
        yield return new RoleRequirement(Roles.Attendee);
        yield return new BookingOwnerRequirement(query.BookingId);
    }
}