using Eventify.Application.Common.Abstractions.Data;

namespace Eventify.Application.Events.Commands.UpdateSlug;

public sealed record UpdateEventSlugCommand : ICommand<Success>, ITransactional
{
    public required Guid EventId { get; init; }

    public required string Slug { get; init; }
}