using Eventify.Domain.Common.Repository;
using Eventify.Domain.Producers.ValueObjects;
using Eventify.Domain.Users.ValueObjects;

namespace Eventify.Domain.Producers.Repository;

public interface IProducerRepository : IRepository<Producer, ProducerId>
{
    Task<Producer?> GetByUserAsync(UserId userId, CancellationToken cancellationToken = default);
    
    Task<ProducerId?> GetIdByUserAsync(UserId userId, CancellationToken cancellationToken = default);
    
    Task<bool> ExistsByUserAsync(UserId userId, CancellationToken cancellationToken = default);
}