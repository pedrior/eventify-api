namespace Eventify.Application.Attendees.Queries.GetBookings;

public sealed record GetAttendeeBookingsQuery : IQuery<IEnumerable<Guid>>;