using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.Attendees.Queries.GetBookings;

internal sealed class GetAttendeeBookingsQueryHandler(
    IUser user,
    IAttendeeRepository attendeeRepository
) : IQueryHandler<GetAttendeeBookingsQuery, IEnumerable<Guid>>
{
    public async Task<ErrorOr<IEnumerable<Guid>>> Handle(GetAttendeeBookingsQuery _,
        CancellationToken cancellationToken)
    {
        var attendee = await attendeeRepository.GetByUserAsync(user.Id, cancellationToken);
        return attendee is not null
            ? ErrorOrFactory.From(attendee.BookingIds.Select(id => id.Value))
            : throw new ApplicationException("Attendee not found");
    }
}