using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Events.Services;

namespace Eventify.Infrastructure.Events;

internal sealed class EventSlugUniquenessChecker(IEventRepository eventRepository) : IEventSlugUniquenessChecker
{
    public async Task<bool> IsUniqueAsync(Slug slug, CancellationToken cancellationToken) =>
        !await eventRepository.ExistsAsync(slug, cancellationToken);
}