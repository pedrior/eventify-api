using Eventify.Application.Common.Abstractions.Data;

namespace Eventify.Application.Events.Commands.UpdateDetails;

public sealed record UpdateEventDetailsCommand : ICommand<Success>, ITransactional
{
    public required Guid EventId { get; init; }
    
    public required string Name { get; init; }

    public required string Category { get; init; }

    public required string Language { get; init; }

    public required string? Description { get; init; }
}