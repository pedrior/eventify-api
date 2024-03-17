namespace Eventify.Contracts.Account.Requests;

public sealed record RegisterRequest
{
    public string Email { get; init; } = null!;
    
    public string Password { get; init; } = null!;
    
    public string GivenName { get; init; } = null!;
    
    public string FamilyName { get; init; } = null!;
    
    public string? PhoneNumber { get; init; }
    
    public DateOnly? BirthDate { get; init; }
}