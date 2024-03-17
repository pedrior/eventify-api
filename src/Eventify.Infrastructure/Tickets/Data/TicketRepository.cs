using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;
using Eventify.Domain.Tickets.ValueObjects;
using Eventify.Infrastructure.Common.Data;

namespace Eventify.Infrastructure.Tickets.Data;

internal sealed class TicketRepository(DataContext context)
    : Repository<Ticket, TicketId, DataContext>(context), ITicketRepository
{
    public Task<EventId?> GetEventIdAsync(TicketId ticketId, CancellationToken cancellationToken)
    {
        return Set
            .Where(t => t.Id == ticketId)
            .Select(t => t.EventId)
            .SingleOrDefaultAsync(cancellationToken);
    }
    
    public async Task<IReadOnlyCollection<Ticket>> ListByEventAsync(
        EventId eventId,
        CancellationToken cancellationToken)
    {
        var tickets = await Set
            .AsNoTracking()
            .Where(t => t.EventId == eventId)
            .ToListAsync(cancellationToken);

        return tickets.AsReadOnly();
    }
}