using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Producers.Repository;

namespace Eventify.Application.Events.Commands.DeleteEvent;

internal sealed class DeleteEventCommandHandler(
    IEventRepository eventRepository,
    IProducerRepository producerRepository
) : ICommandHandler<DeleteEventCommand, Success>
{
    public async Task<Result<Success>> Handle(DeleteEventCommand command,
        CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAsync(command.EventId, cancellationToken);
        if (@event is null)
        {
            return EventErrors.NotFound(command.EventId);
        }

        var producer = await producerRepository.GetAsync(@event.ProducerId, cancellationToken);
        return producer is not null
            ? await producer.DeleteEvent(@event)
                .ThenAsync(() => producerRepository.UpdateAsync(producer, cancellationToken))
            : throw new ApplicationException($"Producer {@event.ProducerId} not found");
    }
}