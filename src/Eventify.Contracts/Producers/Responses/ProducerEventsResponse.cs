namespace Eventify.Contracts.Producers.Responses;

public sealed record ProducerEventsResponse
{
    public IEnumerable<Guid> Events { get; init; } = new List<Guid>();
    
    public IEnumerable<Guid> DeletedEvents { get; init; } = new List<Guid>();
}