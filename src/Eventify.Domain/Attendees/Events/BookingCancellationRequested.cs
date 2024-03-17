using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Common.Events;

namespace Eventify.Domain.Attendees.Events;

public sealed record BookingCancellationRequested(BookingId BookingId) : IDomainEvent;