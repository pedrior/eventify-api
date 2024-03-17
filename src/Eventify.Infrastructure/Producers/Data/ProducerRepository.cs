using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Producers.ValueObjects;
using Eventify.Domain.Users.ValueObjects;
using Eventify.Infrastructure.Common.Data;

namespace Eventify.Infrastructure.Producers.Data;

internal sealed class ProducerRepository(DataContext context)
    : Repository<Domain.Producers.Producer, ProducerId, DataContext>(context), IProducerRepository
{
    public Task<Domain.Producers.Producer?> GetByUserAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return Set.SingleOrDefaultAsync(p => p.UserId == userId, cancellationToken);
    }

    public Task<ProducerId?> GetIdByUserAsync(UserId userId, CancellationToken cancellationToken = default)
    {
        return Set.Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public Task<bool> ExistsByUserAsync(UserId userId, CancellationToken cancellationToken = default) =>
        Set.AnyAsync(p => p.UserId == userId, cancellationToken);
}