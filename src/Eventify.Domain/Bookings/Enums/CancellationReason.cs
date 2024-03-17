using Eventify.Domain.Common.Enums;

namespace Eventify.Domain.Bookings.Enums;

public sealed class CancellationReason : Enumeration<CancellationReason>
{
    public static readonly CancellationReason AttendeeRequest = new("attendee_request", 1);
    public static readonly CancellationReason EventCancelled = new("event_cancelled", 2);
    public static readonly CancellationReason TicketUnavailable = new("ticket_unavailable", 3);
    public static readonly CancellationReason PaymentFailed = new("payment_failed", 4);
    
    private CancellationReason(string name, int value) : base(name, value)
    {
    }
}