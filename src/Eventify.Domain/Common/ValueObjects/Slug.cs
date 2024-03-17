using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Eventify.Domain.Common.ValueObjects;

public sealed partial class Slug : ValueObject
{
    public Slug(string value) : this(value, true)
    {
    }

    private Slug(string value, bool validate = false)
    {
        if (validate && !IsValid(value))
        {
            throw new ArgumentException("Invalid slug format.", nameof(value));
        }

        Value = value;
    }

    public string Value { get; }

    public static bool IsValid(string value) => SlugRegex().IsMatch(value);

    public static Slug Create(string value, bool randomSuffix = false)
    {
        if (!AlphanumericRegex().IsMatch(value))
        {
            throw new ArgumentException("Must contain at least one alphanumeric character.", nameof(value));
        }

        value = RemoveDiacritics(value.ToLowerInvariant());

        var builder = new StringBuilder();
        var previousWasSeparator = false;

        foreach (var c in value)
        {
            if (c is >= 'a' and <= 'z' or >= '0' and <= '9')
            {
                builder.Append(c);
                previousWasSeparator = false;
            }
            else if (!previousWasSeparator)
            {
                builder.Append('-');
                previousWasSeparator = true;
            }
        }

        if (randomSuffix)
        {
            builder.Append($"-{Guid.NewGuid().ToString("N")[..8]}");
        }

        return new Slug(builder.ToString().Trim('-'));
    }

    private static string RemoveDiacritics(string value)
    {
        var builder = new StringBuilder();

        foreach (var c in value.Normalize(NormalizationForm.FormD))
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) is not UnicodeCategory.NonSpacingMark)
            {
                builder.Append(c);
            }
        }

        return builder.ToString().Normalize(NormalizationForm.FormC);
    }

    public override string ToString() => Value;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    [GeneratedRegex("^[a-z0-9]+(?:-[a-z0-9]+)*$")]
    private static partial Regex SlugRegex();

    [GeneratedRegex("[a-zA-Z0-9]")]
    private static partial Regex AlphanumericRegex();
}