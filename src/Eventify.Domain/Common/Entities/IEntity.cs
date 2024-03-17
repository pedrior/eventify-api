using Eventify.Domain.Common.Events;

namespace Eventify.Domain.Common.Entities;

public interface IEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void RaiseDomainEvent(IDomainEvent @event);

    void ClearDomainEvents();
}