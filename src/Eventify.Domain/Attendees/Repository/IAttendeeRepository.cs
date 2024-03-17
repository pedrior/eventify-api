using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Common.Repository;
using Eventify.Domain.Users.ValueObjects;

namespace Eventify.Domain.Attendees.Repository;

public interface IAttendeeRepository : IRepository<Attendee, AttendeeId>
{
    Task<Attendee?> GetByUserAsync(UserId userId, CancellationToken cancellationToken = default);
    
    Task<AttendeeId?> GetIdByUserAsync(UserId userId, CancellationToken cancellationToken = default);
}