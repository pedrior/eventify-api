using Eventify.Domain.Common.Enums;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.Enums;
using Eventify.Domain.Events.ValueObjects;

namespace Eventify.TestUtils.Constants;

public static partial class Constants
{
    public static class Event
    {
        public static readonly EventId EventId = EventId.New();

        public static readonly Slug Slug = new("test-event");

        public static readonly EventDetails Details = new(
            name: ".NET Conference",
            description: "This time at .NET Conference, we will be discussing the latest trends in .NET development.",
            category: EventCategory.Conference,
            language: Language.English);

        public static readonly Period Period = new(
            start: DateTimeOffset.UtcNow.AddDays(2),
            end: DateTimeOffset.UtcNow.AddDays(8));

        public static readonly EventLocation Location = new(
            name: "Centro de Convenções de João Pessoa",
            address: "Rodovia PB-008, Km 5 s/n Polo Turístico - Cabo Branco",
            zipCode: "58000000",
            city: "João Pessoa",
            state: "PB",
            country: "BR");
    }
}