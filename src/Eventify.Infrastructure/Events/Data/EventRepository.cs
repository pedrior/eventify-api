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

    public async Task<IReadOnlyCollection<Event>> ListPublishedAsync(
        int page,
        int limit,
        string? term,
        CancellationToken cancellationToken = default)
    {
        var query = ApplyTermFilter(
            query: Set.AsNoTracking()
                .Where(e => e.State == EventState.Published),
            term: term);

        var events = await query
            .OrderBy(e => e.PublishedAt)
            .ThenBy(e => e.Details.Name)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return events.AsReadOnly();
    }

    public Task<bool> ExistsAsync(Slug slug, CancellationToken cancellationToken = default) =>
        Set.AnyAsync(e => e.Slug == slug, cancellationToken);

    public Task<bool> IsOwnerAsync(EventId eventId, ProducerId producerId,
        CancellationToken cancellationToken = default) => Set.AnyAsync(
        e => e.Id == eventId && e.ProducerId == producerId, cancellationToken);

    public Task<int> CountPublishedAsync(string? term, CancellationToken cancellationToken = default) =>
        ApplyTermFilter(
                query: Set.Where(e => e.State == EventState.Published),
                term: term)
            .CountAsync(cancellationToken);

    private static IQueryable<Event> ApplyTermFilter(IQueryable<Event> query, string? term)
    {
        if (!string.IsNullOrWhiteSpace(term))
        {
            query = query.Where(e => EF.Functions.ILike(e.Details.Name, $"%{term}%")
                                     || EF.Functions.ILike((string)(object)e.Details.Category, $"%{term}%"));
        }

        return query;
    }
}