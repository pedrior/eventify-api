using Eventify.Application.Common.Abstractions.Persistence;
using Eventify.Application.Common.Abstractions.Requests;

namespace Eventify.Application.Events.Commands.UpdateLocation;

public sealed record UpdateEventLocationCommand : ICommand<Success>, ITransactional
{
    public required Guid EventId { get; init; }

    public required string Name { get; init; }

    public required string Address { get; init; }

    public required string ZipCode { get; init; }

    public required string City { get; init; }

    public required string State { get; init; }

    public required string Country { get; init; }
}