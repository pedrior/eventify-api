using Eventify.Application.Common.Abstractions.Persistence;
using Eventify.Application.Common.Abstractions.Requests;

namespace Eventify.Application.Producers.Commands.UpdateProfile;

public class UpdateProducerProfileCommand : ICommand<Success>, ITransactional
{
    public required string Name { get; init; }
    
    public required string? Bio { get; init; }
    
    public required string Email { get; init; }
    
    public required string PhoneNumber { get; init; }
    
    public required Uri? WebsiteUrl { get; init; }
}