using Eventify.Domain.Common.ValueObjects;

namespace Eventify.Domain.Events.Services;

public interface IEventSlugUniquenessChecker
{
    Task<bool> IsUniqueAsync(Slug slug, CancellationToken cancellationToken);
}