namespace Eventify.Domain.Common.Enums;

public sealed class Language : Enumeration<Language>
{
    public static readonly Language English = new("en", 0);
    public static readonly Language Portuguese = new("pt", 1);

    private Language(string name, int value) : base(name, value)
    {
    }
}