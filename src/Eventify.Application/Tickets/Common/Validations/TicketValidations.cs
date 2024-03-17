using Eventify.Domain.Tickets;

namespace Eventify.Application.Tickets.Common.Validations;

internal static class TicketValidations
{
    public static IRuleBuilderOptions<T, string?> TicketName<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage("Must not be empty.")
            .MaximumLength(TicketConstants.NameMaxLength)
            .WithMessage($"Must be less than {TicketConstants.NameMaxLength} characters.")
            .Matches(@"^[\p{L}\p{N}\p{P} ]+$")
            .WithMessage("Must contain only letters, numbers, punctuation, and spaces.");
    }

    public static IRuleBuilderOptions<T, string?> TicketDescription<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule
            .MaximumLength(TicketConstants.DescriptionMaxLength)
            .WithMessage($"Must be less than {TicketConstants.DescriptionMaxLength} characters.")
            .Matches(@"^[\p{L}\p{N}\p{P}\p{Z}\p{S}]+$")
            .WithMessage("Must contain only letters, numbers, punctuation, symbols, and spaces.");
    }
    
    public static IRuleBuilderOptions<T, DateTimeOffset?> TicketSaleDate<T>(this IRuleBuilder<T, DateTimeOffset?> rule)
    {
        return rule
            .Must(v => v is null || v.Value.Offset == TimeSpan.Zero)
            .WithMessage("Must be in UTC.");
    }
}