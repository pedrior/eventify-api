using Eventify.Contracts.Events.Responses;

namespace Eventify.Application.Events.Commands.CreateEvent;

public sealed record CreateEventCommand : ICommand<EventEditResponse>, ITransactional
{
    public required string Name { get; init; }

    public required string Category { get; init; }

    public required string Language { get; init; }

    public required string? Description { get; init; }

    public required DateTimeOffset PeriodStart { get; init; }

    public required DateTimeOffset PeriodEnd { get; init; }

    public required string LocationName { get; init; }

    public required string LocationAddress { get; init; }

    public required string LocationZipCode { get; init; }

    public required string LocationCity { get; init; }

    public required string LocationState { get; init; }

    public required string LocationCountry { get; init; }
}