using Eventify.Application.Common.Abstractions.Persistence;
using Eventify.Application.Common.Abstractions.Requests;

namespace Eventify.Application.Events.Commands.RemovePoster;

public sealed record RemoveEventPosterCommand : ICommand<Success>, ITransactional
{
    public required Guid EventId { get; init; }
}