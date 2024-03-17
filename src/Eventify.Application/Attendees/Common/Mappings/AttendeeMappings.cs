using Eventify.Contracts.Attendees.Responses;
using Eventify.Domain.Attendees;

namespace Eventify.Application.Attendees.Common.Mappings;

internal sealed class AttendeeMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        MapAttendeeToAttendeeProfileResponse(config);
    }

    private static void MapAttendeeToAttendeeProfileResponse(TypeAdapterConfig config) =>
        config.NewConfig<Attendee, AttendeeProfileResponse>()
            .Map(dest => dest.GivenName, src => src.Details.GivenName)
            .Map(dest => dest.FamilyName, src => src.Details.FamilyName)
            .Map(dest => dest.BirthDate, src => src.Details.BirthDate)
            .Map(dest => dest.Email, src => src.Contact.Email)
            .Map(dest => dest.PhoneNumber, src => src.Contact.PhoneNumber);
}