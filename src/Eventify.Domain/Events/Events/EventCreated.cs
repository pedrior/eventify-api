using Eventify.Domain.Common.Events;

namespace Eventify.Domain.Events.Events;

public sealed record EventCreated(Event Event) : IDomainEvent;