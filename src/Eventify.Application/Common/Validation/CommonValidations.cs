using Eventify.Application.Common.Abstractions.Files;
using Eventify.Application.Common.Extensions;
using Eventify.Domain.Common;

namespace Eventify.Application.Common.Validation;

internal static class CommonValidations
{
    public static IRuleBuilderOptions<T, Guid> Guid<T>(this IRuleBuilder<T, Guid> rule)
    {
        return rule.NotEmpty()
            .WithMessage("Must not be empty.");
    }

    public static IRuleBuilderOptions<T, decimal> Price<T>(this IRuleBuilder<T, decimal> rule)
    {
        return rule.GreaterThanOrEqualTo(0m)
            .WithMessage("Must not be negative.");
    }

    public static IRuleBuilderOptions<T, int> Quantity<T>(this IRuleBuilder<T, int> rule)
    {
        return rule.GreaterThanOrEqualTo(0)
            .WithMessage("Must not be negative.");
    }
    
    public static IRuleBuilderOptions<T, string> Language<T>(this IRuleBuilder<T, string> rule)
    {
        return rule.NotEmpty()
            .WithMessage("Must not be empty.")
            .Must(x => Domain.Common.Enums.Language.IsDefined(x, true))
            .WithMessage("Must be a supported language.");
    }

    public static IRuleBuilderOptions<T, DateTimeOffset> UtcDateTine<T>(this IRuleBuilder<T, DateTimeOffset> rule, 
        bool isFuture = true)
    {
        return rule.GreaterThan(DateTimeOffset.UtcNow)
            .When(_ => isFuture)
            .WithMessage("Must be in the future.")
            .Must(dt => dt.Offset == TimeSpan.Zero)
            .WithMessage("Must be in UTC.");
    }

    public static IRuleBuilderOptions<T, IFile> Image<T>(this IRuleBuilder<T, IFile> rule)
    {
        return rule
            .Must(x => x.Stream.IsImage())
            .WithMessage("Must be a valid image file (PNG, JPG, JPEG or BMP).");
    }
    
    public static IRuleBuilderOptions<T, string?> Email<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage("Must not be empty.")
            .MaximumLength(255)
            .WithMessage("Must not exceed 255 characters.")
            .Matches(@"^(?!.*(?:\.-|-\.))[^@]+@[^\W_](?:[\w-]*[^\W_])?(?:\.[^\W_](?:[\w-]*[^\W_])?)+$")
            .WithMessage("Must be a valid email address.");
    }

    public static IRuleBuilderOptions<T, string?> Password<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage("Must not be empty.")
            .Length(8, 20)
            .WithMessage("Must be between 8 and 20 characters.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).+$")
            .WithMessage("Must contain at least one lowercase letter, one uppercase letter and one number.");
    }

    public static IRuleBuilderOptions<T, string?> PhoneNumber<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule
            .MaximumLength(CommonConstants.PhoneNumberMaxLength)
            .WithMessage($"Must not exceed {CommonConstants.PhoneNumberMaxLength} characters.")
            .Matches(@"^\+?[0-9]+$")
            .WithMessage("Must contain only numbers and a leading plus sign.");
    }

    public static IRuleBuilderOptions<T, Uri?> Url<T>(this IRuleBuilder<T, Uri?> rule)
    {
        return rule
            .Must(v => v is null || v.ToString().Length <= CommonConstants.UrlMaxLength)
            .WithMessage($"Must not exceed {CommonConstants.UrlMaxLength} characters.")
            .Must(v => v is null || v.Scheme == Uri.UriSchemeHttp || v.Scheme == Uri.UriSchemeHttps)
            .WithMessage("Must be a valid URL with the HTTP or HTTPS scheme.");
    }

    public static IRuleBuilderOptions<T, string?> PersonName<T>(this IRuleBuilder<T, string?> rule)
    {
        return rule
            .NotEmpty()
            .WithMessage("Must not be empty.")
            .MaximumLength(30)
            .WithMessage("Must not exceed 30 characters.")
            .Matches(@"^(?!.*['.]$)[\p{L}'. }]+$")
            .WithMessage("Must contain only letters, apostrophes, periods and spaces. " +
                         "Must not begin or end with an apostrophe or period.");
    }

    public static IRuleBuilderOptions<T, DateOnly?> PersonBirthDate<T>(this IRuleBuilder<T, DateOnly?> rule)
    {
        return rule
            .Must(x => x is null || x < DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Must be a date in the past.");
    }
}