using Eventify.Application.Attendees.Common.Errors;
using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Application.Common.Abstractions.Storage;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.Attendees.Commands.UploadPicture;

internal sealed class UploadAttendeePictureCommandHandler(
    IUser user,
    IAttendeeRepository attendeeRepository,
    IStorageService storageService
) : ICommandHandler<UploadAttendeePictureCommand, Success>
{
    public async Task<Result<Success>> Handle(UploadAttendeePictureCommand command,
        CancellationToken cancellationToken)
    {
        var attendee = await attendeeRepository.GetByUserAsync(user.Id, cancellationToken);
        if (attendee is null)
        {
            throw new ApplicationException("Attendee not found");
        }

        var pictureUrl = await storageService.UploadAsync(
            StorageKeys.AttendeePicture(attendee.Id),
            command.Picture,
            cancellationToken);

        if (pictureUrl is null)
        {
            return AttendeeErrors.PictureUploadFailed;
        }

        return await attendee.SetPicture(pictureUrl)
            .ThenAsync(() => attendeeRepository.UpdateAsync(attendee, cancellationToken));
    }
}