using Eventify.Domain.Events.ValueObjects;

namespace Eventify.Domain.Producers.Errors;

internal static class ProducerErrors
{
    public static Error EventAlreadyAdded(EventId eventId) => Error.Conflict(
        "The producer already has the event",
        code: "producer.duplicate_event",
        metadata: new() { ["event_id"] = eventId.Value });
    
    public static Error EventNotFound(EventId eventId) => Error.NotFound(
        "The producer does not have the event",
        code: "producer.event_not_found",
        metadata: new() { ["event_id"] = eventId.Value });
    
    public static Error EventAlreadyPublished(EventId eventId) => Error.Conflict(
        "The producer has already published the event",
        code: "producer.event_already_published",
        metadata: new() { ["event_id"] = eventId.Value });

    public static Error CannotDeleteOngoingEvent(EventId eventId) => Error.Failure(
        "The producer cannot delete an event that is currently ongoing",
        code: "producer.cannot_delete_ongoing_event",
        metadata: new() { ["event_id"] = eventId.Value });
}