using Eventify.Application.Common.Abstractions.Data;

namespace Eventify.Application.Events.Commands.RemovePoster;

public sealed record RemoveEventPosterCommand : ICommand<Deleted>, ITransactional
{
    public required Guid EventId { get; init; }
}