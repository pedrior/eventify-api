namespace Eventify.Application.Common.Abstractions.Data;

public interface IUnitOfWork
{
    Task<Guid> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
}