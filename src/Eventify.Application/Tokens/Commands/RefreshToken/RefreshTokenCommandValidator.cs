namespace Eventify.Application.Tokens.Commands.RefreshToken;

internal sealed class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Must not be empty.");
        
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Must not be empty.");
    }
}