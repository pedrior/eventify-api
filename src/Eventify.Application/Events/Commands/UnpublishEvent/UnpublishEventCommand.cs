using Eventify.Application.Common.Abstractions.Persistence;
using Eventify.Application.Common.Abstractions.Requests;

namespace Eventify.Application.Events.Commands.UnpublishEvent;

public sealed record UnpublishEventCommand: ICommand<Success>, ITransactional
{
    public required Guid EventId { get; init; }
}