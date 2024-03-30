using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.Repository;

namespace Eventify.Application.Events.Commands.UpdatePeriod;

internal sealed class UpdateEventPeriodCommandHandler(IEventRepository eventRepository)
    : ICommandHandler<UpdateEventPeriodCommand, Success>
{
    public async Task<Result<Success>> Handle(UpdateEventPeriodCommand command,
        CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAsync(command.EventId, cancellationToken);
        if (@event is null)
        {
            return EventErrors.NotFound(command.EventId);
        }

        var period = new Period(command.Start, command.End);

        return await @event.UpdatePeriod(period)
            .ThenAsync(() => eventRepository.UpdateAsync(@event, cancellationToken));
    }
}