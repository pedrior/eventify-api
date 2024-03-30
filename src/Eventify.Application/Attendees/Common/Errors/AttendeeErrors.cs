namespace Eventify.Application.Attendees.Common.Errors;

internal static class AttendeeErrors
{
    public static readonly Error PictureUploadFailed = Error.Failure(
        "An error occurred while uploading the attendee profile picture",
        code: "attendee.picture_upload_failed");

    public static readonly Error PictureDeletionFailed = Error.Failure(
        "An error occurred while deleting the attendee profile picture",
        code: "attendee.picture_deletion_failed");
}