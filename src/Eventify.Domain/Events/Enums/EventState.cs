using Eventify.Domain.Common.Enums;

namespace Eventify.Domain.Events.Enums;

public sealed class EventState : Enumeration<EventState>
{
    public static readonly EventState Draft = new("draft", 1, s => s == Published);
    public static readonly EventState Published = new("published", 2, s => s == Draft);

    private readonly Func<EventState, bool>? transition;

    private EventState(string name, int value, Func<EventState, bool>? transition = null) : base(name, value)
    {
        this.transition = transition;
    }
    
    public bool CanTransitionTo(EventState state) => transition?.Invoke(state) ?? false;
}