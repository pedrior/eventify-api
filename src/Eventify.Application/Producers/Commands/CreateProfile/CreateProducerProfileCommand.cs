using Eventify.Application.Common.Abstractions.Data;

namespace Eventify.Application.Producers.Commands.CreateProfile;

public sealed record CreateProducerProfileCommand : ICommand<Created>, ITransactional
{
    public required string Name { get; init; }
    
    public required string? Bio { get; init; }
    
    public required string Email { get; init; }
    
    public required string PhoneNumber { get; init; }
    
    public required Uri? WebsiteUrl { get; init; }
}