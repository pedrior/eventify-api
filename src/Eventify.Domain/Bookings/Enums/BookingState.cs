using Eventify.Domain.Common.Enums;

namespace Eventify.Domain.Bookings.Enums;

public sealed class BookingState : Enumeration<BookingState>
{
    public static readonly BookingState Pending = new("pending", 1,
        s => s == Paid || s == Confirmed || s == Cancelled);

    public static readonly BookingState Paid = new("paid", 2,
        s => s == Confirmed || s == Cancelled);

    public static readonly BookingState Confirmed = new("confirmed", 3,
        s => s == Cancelled);

    public static readonly BookingState Cancelled = new("cancelled", 4);

    private readonly Func<BookingState, bool>? transition;

    private BookingState(string name, int value, Func<BookingState, bool>? transition = null) : base(name, value)
    {
        this.transition = transition;
    }

    public bool IsActive() => this == Pending || this == Paid || this == Confirmed;

    public bool CanTransitionTo(BookingState newState) => transition?.Invoke(newState) ?? false;
}