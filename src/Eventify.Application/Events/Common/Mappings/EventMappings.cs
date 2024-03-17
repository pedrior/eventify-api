using Eventify.Application.Events.Commands.CreateEvent;
using Eventify.Contracts.Events.Requests;
using Eventify.Contracts.Events.Responses;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Enums;

namespace Eventify.Application.Events.Common.Mappings;

internal sealed class EventMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<EventState, string>()
            .ConstructUsing(src => src.Name);

        config.NewConfig<EventCategory, string>()
            .ConstructUsing(src => src.Name);

        config.NewConfig<Event, EventResponse>()
            .Map(dest => dest.Name, src => src.Details.Name)
            .Map(dest => dest.Category, src => src.Details.Category)
            .Map(dest => dest.Language, src => src.Details.Language)
            .Map(dest => dest.Description, src => src.Details.Description);
        
        config.NewConfig<Event, EventEditResponse>()
            .Map(dest => dest.Name, src => src.Details.Name)
            .Map(dest => dest.Category, src => src.Details.Category)
            .Map(dest => dest.Language, src => src.Details.Language)
            .Map(dest => dest.Description, src => src.Details.Description);

        config.NewConfig<CreateEventRequest, CreateEventCommand>()
            .Map(dest => dest.PeriodStart, src => src.Period.Start)
            .Map(dest => dest.PeriodEnd, src => src.Period.End)
            .Map(dest => dest.LocationName, src => src.Location.Name)
            .Map(dest => dest.LocationAddress, src => src.Location.Address)
            .Map(dest => dest.LocationZipCode, src => src.Location.ZipCode)
            .Map(dest => dest.LocationCity, src => src.Location.City)
            .Map(dest => dest.LocationState, src => src.Location.State)
            .Map(dest => dest.LocationCountry, src => src.Location.Country);
    }
}