using Eventify.Domain.Common.Events;

namespace Eventify.Domain.Bookings.Events;

public sealed record BookingPaid(Booking Booking) : IDomainEvent;