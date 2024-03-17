using Eventify.Domain.Common;
using Eventify.Domain.Events;

namespace Eventify.Application.Events.Common.Validations;

internal static class EventValidations
{
    private const int LocationNameMaxLength = 50;
    private const int LocationAddressMaxLength = 100;
    private const int LocationZipCodeMaxLength = 8;
    private const int LocationCityMaxLength = 50;
    private const int LocationStateMaxLength = 2;
    private const int LocationCountryMaxLength = 2;

    private const string LocationNameRegex = """^[\p{L}\d\s\-\'"":;,!@#$%&*()=/*-+.\/\[\]{}?/°º_`´]+$""";

    private const string LocationZipCodeRegex = @"^\d+$";

    public static IRuleBuilderOptions<T, string> EventName<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.NotEmpty()
            .WithMessage("Must not be empty.")
            .MaximumLength(EventConstants.NameMaxLength)
            .WithMessage($"Must be less than {EventConstants.NameMaxLength} characters long.");
    }

    public static IRuleBuilderOptions<T, string> EventCategory<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.NotEmpty()
            .WithMessage("Must not be empty.")
            .Must(t => Domain.Events.Enums.EventCategory.IsDefined(t, true))
            .WithMessage("Must be a supported category.");
    }

    public static IRuleBuilderOptions<T, string> EventLocationName<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.NotEmpty()
            .WithMessage("Must not be empty.")
            .MaximumLength(LocationNameMaxLength)
            .WithMessage($"Must be less than {LocationNameMaxLength} characters long.")
            .Matches(LocationNameRegex)
            .WithMessage("Must contain only letters, numbers and special characters.");
    }

    public static IRuleBuilderOptions<T, string> EventLocationAddress<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.NotEmpty()
            .WithMessage("Must not be empty.")
            .MaximumLength(LocationAddressMaxLength)
            .WithMessage($"Must be less than {LocationAddressMaxLength} characters long.")
            .Matches(LocationNameRegex)
            .WithMessage("Must contain only letters, numbers and special characters.");
    }

    public static IRuleBuilderOptions<T, string> EventLocationZipCode<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.NotEmpty()
            .WithMessage("Must not be empty.")
            .MaximumLength(LocationZipCodeMaxLength)
            .WithMessage($"Must be {LocationZipCodeMaxLength} characters long.")
            .Matches(LocationZipCodeRegex)
            .WithMessage("Must contain only numbers.");
    }

    public static IRuleBuilderOptions<T, string> EventLocationCity<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.NotEmpty()
            .WithMessage("Must not be empty.")
            .MaximumLength(LocationCityMaxLength)
            .WithMessage($"Must be less than {LocationCityMaxLength} characters long.")
            .Matches(LocationNameRegex)
            .WithMessage("Must contain only letters, numbers and special characters.");
    }

    public static IRuleBuilderOptions<T, string> EventLocationState<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.NotEmpty()
            .WithMessage("Must not be empty.")
            .MaximumLength(LocationStateMaxLength)
            .WithMessage($"Must be less than {LocationStateMaxLength} characters long.")
            .Matches(LocationNameRegex)
            .WithMessage("Must contain only letters, numbers and special characters.");
    }

    public static IRuleBuilderOptions<T, string> EventLocationCountry<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.NotEmpty()
            .WithMessage("Must not be empty.")
            .MaximumLength(LocationCountryMaxLength)
            .WithMessage($"Must be less than {LocationCountryMaxLength} characters long.")
            .Matches(LocationNameRegex)
            .WithMessage("Must contain only letters, numbers and special characters.");
    }

    public static IRuleBuilderOptions<T, string> EventSlug<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.NotEmpty()
            .WithMessage("Must not be empty.")
            .MaximumLength(CommonConstants.SlugMaxLength)
            .WithMessage($"Must not exceed {CommonConstants.SlugMaxLength} characters.")
            .Matches("^[a-z0-9]+(?:-[a-z0-9]+)*$")
            .WithMessage("Must contain only lowercase letters, numbers, dashes and must not start or end with a dash.");
    }

    public static IRuleBuilderOptions<T, string?> EventDescription<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule.MaximumLength(EventConstants.DescriptionMaxLength)
            .WithMessage($"Must not exceed {EventConstants.DescriptionMaxLength} characters.")
            .Matches(@"^[\p{L}\p{N}\p{P}\p{Z}\p{S}]+$")
            .WithMessage("Must contain only letters, numbers, punctuation, symbols, and spaces.");
    }
}