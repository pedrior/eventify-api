using Eventify.Application.Attendees.Common.Errors;
using Eventify.Application.Common.Abstractions.Storage;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.Attendees.Commands.RemovePicture;

internal sealed class RemoveAttendeePictureCommandHandler(
    IUser user,
    IAttendeeRepository attendeeRepository,
    IStorageService storageService
) : ICommandHandler<RemoveAttendeePictureCommand, Deleted>
{
    public async Task<ErrorOr<Deleted>> Handle(RemoveAttendeePictureCommand command,
        CancellationToken cancellationToken)
    {
        var attendee = await attendeeRepository.GetByUserAsync(user.Id, cancellationToken);
        if (attendee is null)
        {
            throw new ApplicationException("Attendee not found");
        }

        if (attendee.PictureUrl is null)
        {
            return Result.Deleted;
        }

        var pictureKey = StorageKeys.AttendeePicture(attendee.Id);
        if (!await storageService.DeleteAsync(pictureKey, cancellationToken))
        {
            return AttendeeErrors.PictureDeletionFailed;
        }

        return await attendee.RemovePicture()
            .ThenAsync(_ => attendeeRepository.UpdateAsync(attendee, cancellationToken));
    }
}