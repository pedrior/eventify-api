namespace Eventify.Application.Common.Abstractions.Persistence;

public interface IUnitOfWork
{
    Task<Guid> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
}