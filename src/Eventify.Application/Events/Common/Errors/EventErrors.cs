using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.ValueObjects;

namespace Eventify.Application.Events.Common.Errors;

internal static class EventErrors
{
    public static readonly Error PosterUploadFailed = Error.Failure(
        "An error occurred while uploading the event poster",
        code: "event.poster_upload_failed");

    public static readonly Error PosterDeletionFailed = Error.Failure(
        "An error occurred while deleting the event poster",
        code: "event.poster_deletion_failed");

    public static Error NotFound(EventId eventId) => Error.NotFound(
        "Event not found",
        code: "event.not_found",
        metadata: new() { ["event_id"] = eventId.Value });

    public static Error NotFound(Slug slug) => Error.NotFound(
        "Event not found",
        code: "event.not_found",
        metadata: new() { ["slug"] = slug.Value });
}