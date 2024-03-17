using Eventify.Domain.Common.Events;

namespace Eventify.Application.Common.Abstractions.CQRS;

public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IDomainEvent;