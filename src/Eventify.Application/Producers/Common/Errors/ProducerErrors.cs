namespace Eventify.Application.Producers.Common.Errors;

internal static class ProducerErrors
{
    public static readonly Error ProfileAlreadyCreated = Error.Conflict(
        "The producer profile already exists",
        code: "producer.profile_already_created");
    
    public static readonly Error PictureUploadFailed = Error.Failure(
        "An error occurred while uploading the producer profile picture",
        code: "producer.picture_upload_failed");

    public static readonly Error PictureDeletionFailed = Error.Failure(
        "An error occurred while deleting the producer profile picture",
        code: "producer.picture_deletion_failed");
    
}