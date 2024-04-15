using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Users;

namespace Eventify.Application.Attendees.Commands.UpdateProfile;

internal sealed class UpdateAttendeeProfileCommandHandler(
    IUser user,
    IAttendeeRepository attendeeRepository
) : ICommandHandler<UpdateAttendeeProfileCommand, Success>
{
    public async Task<Result<Success>> Handle(UpdateAttendeeProfileCommand command,
        CancellationToken cancellationToken)
    {
        var attendee = await attendeeRepository.GetByUserAsync(user.Id, cancellationToken);
        if (attendee is null)
        {
            throw new ApplicationException("Attendee not found");
        }

        var details = new AttendeeDetails(command.GivenName, command.FamilyName, command.BirthDate);
        var contact = new AttendeeContact(attendee.Contact.Email, command.PhoneNumber);

        return await attendee.UpdateProfile(details, contact)
            .ThenAsync(() => attendeeRepository.UpdateAsync(attendee, cancellationToken));
    }
}