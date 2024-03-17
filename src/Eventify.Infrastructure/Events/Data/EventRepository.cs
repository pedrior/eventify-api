using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Enums;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Producers.ValueObjects;
using Eventify.Infrastructure.Common.Data;

namespace Eventify.Infrastructure.Events.Data;

internal sealed class EventRepository(DataContext context)
    : Repository<Event, EventId, DataContext>(context), IEventRepository
{
    public Task<Event?> GetPublishedAsync(EventId id, CancellationToken cancellationToken = default) =>
        Set.SingleOrDefaultAsync(e => e.Id == id && e.State == EventState.Published, cancellationToken);

    public Task<Event?> GetPublishedAsync(Slug slug, CancellationToken cancellationToken = default) =>
        Set.SingleOrDefaultAsync(e => e.Slug == slug && e.State == EventState.Published, cancellationToken);

    public Task<bool> ExistsAsync(Slug slug, CancellationToken cancellationToken = default) =>
        Set.AnyAsync(e => e.Slug == slug, cancellationToken);

    public Task<bool> IsOwnerAsync(EventId eventId, ProducerId producerId,
        CancellationToken cancellationToken = default) => Set.AnyAsync(
        e => e.Id == eventId && e.ProducerId == producerId, cancellationToken);
}