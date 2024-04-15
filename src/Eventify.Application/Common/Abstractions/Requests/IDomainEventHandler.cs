using Eventify.Domain.Common.Events;

namespace Eventify.Application.Common.Abstractions.Requests;

public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IDomainEvent;