using Eventify.Domain.Bookings.Enums;

namespace Eventify.Application.Bookings.Common.Mappings;

internal sealed class BookingMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        MapEnumsToPrimitiveTypes(config);
    }

    private static void MapEnumsToPrimitiveTypes(TypeAdapterConfig config)
    {
        config.NewConfig<BookingState, string>()
            .ConstructUsing(src => src.Name);

        config.NewConfig<CancellationReason, string>()
            .ConstructUsing(src => src.Name);
    }
}