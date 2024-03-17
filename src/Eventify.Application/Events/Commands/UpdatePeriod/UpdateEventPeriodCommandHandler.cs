using Eventify.Application.Events.Common.Errors;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.Repository;

namespace Eventify.Application.Events.Commands.UpdatePeriod;

internal sealed class UpdateEventPeriodCommandHandler(IEventRepository eventRepository)
    : ICommandHandler<UpdateEventPeriodCommand, Updated>
{
    public async Task<ErrorOr<Updated>> Handle(UpdateEventPeriodCommand command,
        CancellationToken cancellationToken)
    {
        var @event = await eventRepository.GetAsync(command.EventId, cancellationToken);
        if (@event is null)
        {
            return EventErrors.NotFound(command.EventId);
        }

        var period = new Period(command.Start, command.End);

        return await @event.UpdatePeriod(period)
            .ThenAsync(_ => eventRepository.UpdateAsync(@event, cancellationToken));
    }
}