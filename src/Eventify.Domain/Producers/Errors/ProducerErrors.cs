using Eventify.Domain.Events.ValueObjects;

namespace Eventify.Domain.Producers.Errors;

internal static class ProducerErrors
{
    public static Error EventAlreadyAdded(EventId eventId) => Error.Conflict(
        code: "producer.duplicate_event",
        description: "The producer already has the event",
        metadata: new() { ["event_id"] = eventId.Value });
    
    public static Error EventNotFound(EventId eventId) => Error.NotFound(
        code: "producer.event_not_found",
        description: "The producer does not have the event",
        metadata: new() { ["event_id"] = eventId.Value });
    
    public static Error EventAlreadyPublished(EventId eventId) => Error.Conflict(
        code: "producer.event_already_published",
        description: "The producer has already published the event",
        metadata: new() { ["event_id"] = eventId.Value });

    public static Error CannotDeleteOngoingEvent(EventId eventId) => Error.Failure(
        code: "producer.cannot_delete_ongoing_event",
        description: "The producer cannot delete an event that is currently ongoing",
        metadata: new() { ["event_id"] = eventId.Value });
}