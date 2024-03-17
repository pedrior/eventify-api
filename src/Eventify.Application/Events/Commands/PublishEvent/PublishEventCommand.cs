using Eventify.Application.Common.Abstractions.Data;

namespace Eventify.Application.Events.Commands.PublishEvent;

public sealed record PublishEventCommand : ICommand<Success>, ITransactional
{
    public required Guid EventId { get; init; }
}