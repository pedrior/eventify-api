using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Producers.Repository;

namespace Eventify.Application.Events.Commands.UnpublishEvent;

internal sealed class UnpublishEventCommandHandler(
    IEventRepository eventRepository,
    IProducerRepository producerRepository
) : ICommandHandler<UnpublishEventCommand, Success>
{
    public async Task<Result<Success>> Handle(UnpublishEventCommand command,
        CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAsync(command.EventId, cancellationToken);
        if (@event is null)
        {
            return EventErrors.NotFound(command.EventId);
        }

        var producer = await producerRepository.GetAsync(@event.ProducerId, cancellationToken);
        return producer is not null
            ? await producer.UnpublishEvent(@event)
                .ThenAsync(() => eventRepository.UpdateAsync(@event, cancellationToken))
            : throw new ApplicationException($"Producer {@event.ProducerId} not found");
    }
}