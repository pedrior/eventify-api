using Eventify.Contracts.Producers.Responses;
using Eventify.Domain.Producers;

namespace Eventify.Application.Producers.Common.Mappings;

internal sealed class ProducerMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        MapProducerToProducerProfileResponse(config);

        MapProducerToProducerEventsResponse(config);
    }

    private static void MapProducerToProducerProfileResponse(TypeAdapterConfig config) =>
        config.NewConfig<Producer, ProducerProfileResponse>()
            .Map(dest => dest.Name, src => src.Details.Name)
            .Map(dest => dest.Bio, src => src.Details.Bio)
            .Map(dest => dest.Email, src => src.Contact.Email)
            .Map(dest => dest.PhoneNumber, src => src.Contact.PhoneNumber);

    private static void MapProducerToProducerEventsResponse(TypeAdapterConfig config) =>
        config.NewConfig<Producer, ProducerEventsResponse>()
            .Map(dest => dest.Events, src => src.EventIds)
            .Map(dest => dest.DeletedEvents, src => src.DeletedEventIds);
}