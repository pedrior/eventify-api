namespace Eventify.Application.Producers.Common.Errors;

internal static class ProducerErrors
{
    public static readonly Error ProfileAlreadyCreated = Error.Conflict(
        code: "producer.profile_already_created",
        description: "The producer profile already exists");
    
    public static readonly Error PictureUploadFailed = Error.Failure(
        code: "producer.picture_upload_failed",
        description: "An error occurred while uploading the producer profile picture");

    public static readonly Error PictureDeletionFailed = Error.Failure(
        code: "producer.picture_deletion_failed",
        description: "An error occurred while deleting the producer profile picture");
    
}