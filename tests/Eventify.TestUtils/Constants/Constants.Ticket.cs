using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.TestUtils.Constants;

public static partial class Constants
{
    public static class Ticket
    {
        public const string Name = "Test Ticket";

        public const string Description = "Test Ticket Description";

        public static readonly TicketId TicketId = TicketId.New();

        public static readonly Money Price = new(30m);

        public static readonly Quantity Quantity = new(50);
        
        public static readonly Quantity QuantityPerSale = new(1);

        public static readonly DateTimeOffset? SaleStart = null;
        
        public static readonly DateTimeOffset? SaleEnd = null;
    }
}