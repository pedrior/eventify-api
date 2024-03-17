using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Common.Events;

namespace Eventify.Domain.Bookings.Events;

public sealed record BookingCancelled(Booking Booking, CancellationReason Reason) : IDomainEvent;