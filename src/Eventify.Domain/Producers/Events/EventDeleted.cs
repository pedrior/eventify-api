using Eventify.Domain.Common.Events;
using Eventify.Domain.Events;

namespace Eventify.Domain.Producers.Events;

public sealed record EventDeleted(Event Event) : IDomainEvent;