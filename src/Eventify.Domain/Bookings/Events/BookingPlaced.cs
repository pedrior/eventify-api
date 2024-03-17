using Eventify.Domain.Common.Events;

namespace Eventify.Domain.Bookings.Events;

public sealed record BookingPlaced(Booking Booking) : IDomainEvent;