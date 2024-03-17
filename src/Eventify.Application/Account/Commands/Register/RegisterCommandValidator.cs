using Eventify.Application.Common.Validation;

namespace Eventify.Application.Account.Commands.Register;

internal sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .Email();

        RuleFor(x => x.Password)
            .Password();

        RuleFor(x => x.GivenName)
            .PersonName();

        RuleFor(x => x.FamilyName)
            .PersonName();

        RuleFor(x => x.PhoneNumber)
            .PhoneNumber();

        RuleFor(x => x.BirthDate)
            .PersonBirthDate();
    }
}