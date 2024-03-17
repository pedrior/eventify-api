namespace Eventify.Contracts.Events.Responses;

public sealed record EventProducerResponse
{
    public string Name { get; init; } = null!;
    
    public string? Bio  { get; init; }
    
    public string Email { get; init; } = null!;
    
    public string PhoneNumber { get; init; } = null!;
    
    public string? PictureUrl { get; init; }
    
    public string? WebsiteUrl { get; init; }
}