using Eventify.Domain.Common.Events;
using Eventify.Domain.Events.ValueObjects;

namespace Eventify.Domain.Events.Events;

public sealed record EventUnpublished(EventId EventId) : IDomainEvent;