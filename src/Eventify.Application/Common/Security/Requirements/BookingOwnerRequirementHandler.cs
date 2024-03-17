using Eventify.Application.Common.Abstractions.Security;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.Common.Security.Requirements;

internal sealed class BookingOwnerRequirementHandler(
    IUser user,
    IAttendeeRepository attendeeRepository,
    IBookingRepository bookingRepository
) : IRequirementHandler<BookingOwnerRequirement>
{
    public async Task<AuthorizationResult> HandleAsync(BookingOwnerRequirement requirement,
        CancellationToken cancellationToken = default)
    {
        var attendeeId = await attendeeRepository.GetIdByUserAsync(user.Id, cancellationToken);
        if (attendeeId is null)
        {
            // Let request handlers handle non-existing attendees
            return AuthorizationResult.Success();
        }

        return await bookingRepository.IsOwnerAsync(requirement.BookingId, attendeeId, cancellationToken)
            ? AuthorizationResult.Success()
            : AuthorizationResult.Failure("User failed to meet the booking owner requirement.");
    }
}