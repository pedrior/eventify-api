using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Common.ValueObjects;

namespace Eventify.TestUtils.Constants;

public static partial class Constants
{
    public static class Booking
    {
        public static readonly BookingId BookingId = BookingId.New();
        
        public static readonly Money TotalPrice = new(30m);

        public static readonly Quantity TotalQuantity = new(1);
    }
}