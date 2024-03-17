using Eventify.Domain.Common.Enums;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.Enums;

namespace Eventify.Domain.Events.ValueObjects;

public sealed class EventDetails(
    string name,
    EventCategory category,
    Language language,
    string? description = null) : ValueObject
{
    public string Name { get; } = name;

    public EventCategory Category { get; } = category;

    public Language Language { get; } = language;

    public string? Description { get; } = description;

    public override string ToString() => $"{Name} ({Category} - {Language})";

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
        yield return Category;
        yield return Language;
        yield return Description;
    }
}