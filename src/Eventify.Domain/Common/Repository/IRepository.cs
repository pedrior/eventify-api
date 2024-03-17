using Eventify.Domain.Common.Entities;

namespace Eventify.Domain.Common.Repository;

public interface IRepository<TEntity, in TId> where TEntity : class, IEntity<TId>, IAggregateRoot where TId : notnull
{
    Task<TEntity?> GetAsync(TId id, CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<TEntity>> ListAsync(CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(TId id, CancellationToken cancellationToken = default);

    Task<int> CountAsync(CancellationToken cancellationToken = default);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);

    Task RemoveAsync(TEntity entity, CancellationToken cancellationToken = default);
}