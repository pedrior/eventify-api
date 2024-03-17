using Eventify.Application.Common.Validation;

namespace Eventify.Application.Attendees.Commands.UpdateProfile;

internal sealed class UpdateAttendeeProfileCommandValidator : AbstractValidator<UpdateAttendeeProfileCommand>
{
    public UpdateAttendeeProfileCommandValidator()
    {
        RuleFor(x => x.GivenName)
            .PersonName();

        RuleFor(x => x.FamilyName)
            .PersonName();

        RuleFor(x => x.BirthDate)
            .PersonBirthDate();

        RuleFor(x => x.PhoneNumber)
            .PhoneNumber();
    }
}