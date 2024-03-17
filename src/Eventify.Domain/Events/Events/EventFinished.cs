using Eventify.Domain.Common.Events;

namespace Eventify.Domain.Events.Events;

public sealed record EventFinished(Event Event) : IDomainEvent;