namespace Eventify.Contracts.Events.Requests;

public sealed record CreateEventLocationRequest
{
    public string Name { get; init; } = null!;

    public string Address { get; init; } = null!;

    public string ZipCode { get; init; } = null!;

    public string City { get; init; } = null!;

    public string State { get; init; } = null!;

    public string Country { get; init; } = null!;
}