using Eventify.Domain.Common.Events;

namespace Eventify.Domain.Events.Events;

public sealed record EventPublished(Event Event) : IDomainEvent;