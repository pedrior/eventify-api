using Eventify.Application.Common.Abstractions.Persistence;

namespace Eventify.Application.Events.Commands.DeleteEvent;

public sealed record DeleteEventCommand : ICommand<Success>, ITransactional
{
    public required Guid EventId { get; init; }
}