using Eventify.Domain.Common.Events;

namespace Eventify.Domain.Attendees.Events;

public sealed record AttendeeCreated(Attendee Attendee) : IDomainEvent;