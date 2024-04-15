using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Users.ValueObjects;
using Eventify.Infrastructure.Common.Persistence;

namespace Eventify.Infrastructure.Attendees.Data;

internal sealed class AttendeeRepository(DataContext context)
    : Repository<Attendee, AttendeeId, DataContext>(context), IAttendeeRepository
{
    public Task<Attendee?> GetByUserAsync(UserId userId, CancellationToken cancellationToken = default) =>
        Set.SingleOrDefaultAsync(a => a.UserId == userId, cancellationToken);

    public Task<AttendeeId?> GetIdByUserAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return Set
            .Where(a => a.UserId == userId)
            .Select(a => a.Id)
            .SingleOrDefaultAsync(cancellationToken);
    }
}