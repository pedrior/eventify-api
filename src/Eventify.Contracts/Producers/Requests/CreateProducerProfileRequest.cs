namespace Eventify.Contracts.Producers.Requests;

public sealed record CreateProducerProfileRequest
{
    public string Name { get; init; } = null!;
    
    public string? Bio { get; init; } = null!;
    
    public string Email { get; init; } = null!;
    
    public string PhoneNumber { get; init; } = null!;
    
    public Uri? WebsiteUrl { get; init; }
}