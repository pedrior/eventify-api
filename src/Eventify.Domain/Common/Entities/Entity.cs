using Eventify.Domain.Common.Events;

namespace Eventify.Domain.Common.Entities;

public abstract class Entity : IEntity
{
    private readonly List<IDomainEvent> domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents => domainEvents.AsReadOnly();

    public void RaiseDomainEvent(IDomainEvent @event) => domainEvents.Add(@event);

    public void ClearDomainEvents() => domainEvents.Clear();
}