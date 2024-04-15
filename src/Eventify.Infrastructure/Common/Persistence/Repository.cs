using Eventify.Domain.Common.Entities;
using Eventify.Domain.Common.Repository;

namespace Eventify.Infrastructure.Common.Persistence;

internal abstract class Repository<TEntity, TId, TContext>(TContext context) : IRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>, IAggregateRoot
    where TId : notnull
    where TContext : DbContext
{
    protected DbSet<TEntity> Set => context.Set<TEntity>();

    public async Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default) =>
        await Set.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyCollection<TEntity>> ListAsync(CancellationToken cancellationToken = default) =>
        (await Set.ToListAsync(cancellationToken)).AsReadOnly();

    public Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default) =>
        Set.AnyAsync(e => e.Id.Equals(id), cancellationToken);

    public Task<int> CountAsync(CancellationToken cancellationToken = default) =>
        Set.CountAsync(cancellationToken);

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Set.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        // Set.Update(entity);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        Set.Remove(entity); 
        await context.SaveChangesAsync(cancellationToken);
    }
}