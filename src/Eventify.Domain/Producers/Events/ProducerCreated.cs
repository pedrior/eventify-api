using Eventify.Domain.Common.Events;

namespace Eventify.Domain.Producers.Events;

public sealed record ProducerCreated(Producer Producer) : IDomainEvent;