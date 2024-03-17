using Eventify.Domain.Common.Enums;

namespace Eventify.Domain.Events.Enums;

public sealed class EventCategory : Enumeration<EventCategory>
{
    public static readonly EventCategory Conference = new("conference", "Conferências", 1);
    public static readonly EventCategory Expo = new("expo", "Exposições", 2);
    public static readonly EventCategory Festival = new("festival", "Festivais", 3);
    public static readonly EventCategory Concert = new("concert", "Concertos", 4);
    public static readonly EventCategory Party = new("party", "Festas", 5);
    public static readonly EventCategory Sports = new("sports", "Desportos", 6);
    public static readonly EventCategory Workshop = new("workshop", "Workshops", 7);
    public static readonly EventCategory Community = new("community", "Comunidade", 8);
    public static readonly EventCategory Other = new("other", "Outros", 9);
    
    private EventCategory(string name, string description, int value) : base(name, value)
    {
        Description = description;
    }
    
    public string Description { get; }
}