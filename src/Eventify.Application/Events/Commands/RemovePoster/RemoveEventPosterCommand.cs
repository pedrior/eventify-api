using Eventify.Application.Common.Abstractions.Data;

namespace Eventify.Application.Events.Commands.RemovePoster;

public sealed record RemoveEventPosterCommand : ICommand<Success>, ITransactional
{
    public required Guid EventId { get; init; }
}