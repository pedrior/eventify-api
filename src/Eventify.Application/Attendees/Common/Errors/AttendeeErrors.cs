namespace Eventify.Application.Attendees.Common.Errors;

internal static class AttendeeErrors
{
    public static readonly Error PictureUploadFailed = Error.Failure(
        code: "attendee.picture_upload_failed",
        description: "An error occurred while uploading the attendee profile picture");

    public static readonly Error PictureDeletionFailed = Error.Failure(
        code: "attendee.picture_deletion_failed",
        description: "An error occurred while deleting the attendee profile picture");
}