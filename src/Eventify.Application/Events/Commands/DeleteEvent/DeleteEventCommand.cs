using Eventify.Application.Common.Abstractions.Data;

namespace Eventify.Application.Events.Commands.DeleteEvent;

public sealed record DeleteEventCommand : ICommand<Deleted>, ITransactional
{
    public required Guid EventId { get; init; }
}