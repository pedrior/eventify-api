using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Events.ValueObjects;

namespace Eventify.Application.Events.Commands.UpdateLocation;

internal sealed class UpdateEventLocationCommandHandler(IEventRepository eventRepository)
    : ICommandHandler<UpdateEventLocationCommand, Success>
{
    public async Task<Result<Success>> Handle(UpdateEventLocationCommand command,
        CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAsync(command.EventId, cancellationToken);
        if (@event is null)
        {
            return EventErrors.NotFound(command.EventId);
        }

        var location = new EventLocation(
            name: command.Name,
            address: command.Address,
            zipCode: command.ZipCode,
            city: command.City,
            state: command.State,
            country: command.Country);
        
        return await @event.UpdateLocation(location)
            .ThenAsync(() => eventRepository.UpdateAsync(@event, cancellationToken));
    }
}