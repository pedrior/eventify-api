using Eventify.Application.Common.Validation;
using Eventify.Application.Producers.Common.Validations;

namespace Eventify.Application.Producers.Commands.CreateProfile;

internal sealed class CreateProducerProfileCommandValidator : AbstractValidator<CreateProducerProfileCommand>
{
    public CreateProducerProfileCommandValidator()
    {
        RuleFor(x => x.Name)
            .ProducerName();

        RuleFor(x => x.Bio)
            .ProducerBio();

        RuleFor(x => x.Email)
            .Email();

        RuleFor(x => x.PhoneNumber)
            .PhoneNumber();

        RuleFor(x => x.WebsiteUrl)
            .Url();
    }
}