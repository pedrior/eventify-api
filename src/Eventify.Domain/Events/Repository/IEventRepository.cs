using Eventify.Domain.Common.Repository;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Producers.ValueObjects;

namespace Eventify.Domain.Events.Repository;

public interface IEventRepository : IRepository<Event, EventId>
{
    Task<Event?> GetPublishedAsync(EventId id, CancellationToken cancellationToken = default);
    
    Task<Event?> GetPublishedAsync(Slug slug, CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<Event>> ListPublishedAsync(
        int page,
        int limit,
        string? term,
        CancellationToken cancellationToken = default);
    
    Task<bool> ExistsAsync(Slug slug, CancellationToken cancellationToken = default);
    
    Task<bool> IsOwnerAsync(
        EventId eventId,
        ProducerId producerId,
        CancellationToken cancellationToken = default);
    
    Task<int> CountPublishedAsync(string? term, CancellationToken cancellationToken = default);
}