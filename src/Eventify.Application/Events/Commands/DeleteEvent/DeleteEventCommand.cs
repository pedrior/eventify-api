using Eventify.Application.Common.Abstractions.Persistence;
using Eventify.Application.Common.Abstractions.Requests;

namespace Eventify.Application.Events.Commands.DeleteEvent;

public sealed record DeleteEventCommand : ICommand<Success>, ITransactional
{
    public required Guid EventId { get; init; }
}