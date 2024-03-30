using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Bookings.Enums;
using Eventify.Domain.Bookings.Errors;
using Eventify.Domain.Bookings.Events;
using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Common.Entities;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Domain.Bookings;

public sealed class Booking : Entity<BookingId>, IAggregateRoot, ISoftDelete
{
    private Booking(BookingId id) : base(id)
    {
    }

    public required EventId EventId { get; init; }

    public required TicketId TicketId { get; init; }

    public required AttendeeId AttendeeId { get; init; }

    public required Money TotalPrice { get; init; }

    public required Quantity TotalQuantity { get; init; }

    public BookingState State { get; private set; } = default!;

    public CancellationReason? CancellationReason { get; private set; }

    public bool IsDeleted { get; set; }

    public DateTimeOffset PlacedAt { get; private set; }

    public DateTimeOffset? PaidAt { get; private set; }

    public DateTimeOffset? ConfirmedAt { get; private set; }

    public DateTimeOffset? CancelledAt { get; private set; }

    public DateTimeOffset? DeletedAt { get; set; }

    public bool IsCancelled => State == BookingState.Cancelled;

    public static Booking Place(
        BookingId bookingId,
        TicketId ticketId,
        EventId eventId,
        AttendeeId attendeeId,
        Money totalPrice,
        Quantity totalQuantity)
    {
        var booking = new Booking(bookingId)
        {
            TicketId = ticketId,
            EventId = eventId,
            AttendeeId = attendeeId,
            TotalPrice = totalPrice,
            TotalQuantity = totalQuantity,
            State = BookingState.Pending,
            PlacedAt = DateTimeOffset.UtcNow
        };

        booking.RaiseDomainEvent(new BookingPlaced(booking));

        return booking;
    }

    public Result<Success> Pay() => TransitionTo(BookingState.Paid, () =>
    {
        PaidAt = DateTimeOffset.UtcNow;

        RaiseDomainEvent(new BookingPaid(this));
    });

    public Result<Success> Confirm() => TransitionTo(BookingState.Confirmed, () =>
    {
        ConfirmedAt = DateTimeOffset.UtcNow;

        RaiseDomainEvent(new BookingConfirmed(this));
    });

    public Result<Success> Cancel(CancellationReason reason) => TransitionTo(BookingState.Cancelled, () =>
    {
        CancellationReason = reason;
        CancelledAt = DateTimeOffset.UtcNow;

        RaiseDomainEvent(new BookingCancelled(this, reason));
    });

    private Result<Success> TransitionTo(BookingState next, Action action)
    {
        if (!State.CanTransitionTo(next))
        {
            return BookingErrors.InvalidStateOperation(State);
        }

        action();
        State = next;

        return Success.Value;
    }
}