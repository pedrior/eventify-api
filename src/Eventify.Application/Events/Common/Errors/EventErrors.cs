using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.ValueObjects;

namespace Eventify.Application.Events.Common.Errors;

internal static class EventErrors
{
    public static readonly Error PosterUploadFailed = Error.Failure(
        code: "event.poster_upload_failed",
        description: "An error occurred while uploading the event poster");

    public static readonly Error PosterDeletionFailed = Error.Failure(
        code: "event.poster_deletion_failed",
        description: "An error occurred while deleting the event poster");
    
    public static Error NotFound(EventId eventId) => Error.NotFound(
        code: "event.not_found",
        description: "Event not found",
        metadata: new() { ["event_id"] = eventId.Value });

    public static Error NotFound(Slug slug) => Error.NotFound(
        code: "event.not_found",
        description: "Event not found",
        metadata: new() { ["slug"] = slug.Value });
}