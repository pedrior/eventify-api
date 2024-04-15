namespace Eventify.Application.Events.Commands.UnpublishEvent;

public sealed record UnpublishEventCommand: ICommand<Success>, ITransactional
{
    public required Guid EventId { get; init; }
}