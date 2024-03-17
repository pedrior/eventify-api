using Eventify.Domain.Producers;

namespace Eventify.Application.Producers.Common.Validations;

internal static class ProducerValidations
{
    public static IRuleBuilderOptions<T, string> ProducerName<T>(this IRuleBuilder<T, string> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage("Must not be empty.")
            .MaximumLength(ProducerConstants.NameMaxLength)
            .WithMessage($"Must not exceed {ProducerConstants.NameMaxLength} characters.")
            .Matches(@"^[^\s '.-][\p{L}\d '.-]*[^\s' .-]$")
            .WithMessage("Must contain only letters, spaces, apostrophes, dots and hyphens. " +
                         "Must not start or end with spaces, apostrophes, dots or hyphens.");
    }
    
    public static IRuleBuilderOptions<T, string?> ProducerBio<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule
            .MaximumLength(ProducerConstants.BioMaxLength)
            .WithMessage($"Must not exceed {ProducerConstants.BioMaxLength} characters.");
    }
}