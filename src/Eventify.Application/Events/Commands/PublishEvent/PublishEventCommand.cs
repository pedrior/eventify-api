using Eventify.Application.Common.Abstractions.Persistence;
using Eventify.Application.Common.Abstractions.Requests;

namespace Eventify.Application.Events.Commands.PublishEvent;

public sealed record PublishEventCommand : ICommand<Success>, ITransactional
{
    public required Guid EventId { get; init; }
}