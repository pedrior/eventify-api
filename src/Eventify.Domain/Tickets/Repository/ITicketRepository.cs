using Eventify.Domain.Common.Repository;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Domain.Tickets.Repository;

public interface ITicketRepository : IRepository<Ticket, TicketId>
{
    Task<EventId?> GetEventIdAsync(TicketId ticketId, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Ticket>> ListByEventAsync(EventId eventId, CancellationToken cancellationToken = default);
}