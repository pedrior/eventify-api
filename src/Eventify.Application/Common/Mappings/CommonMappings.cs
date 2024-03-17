using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Bookings.ValueObjects;
using Eventify.Domain.Common.Enums;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Producers.ValueObjects;
using Eventify.Domain.Tickets.ValueObjects;

namespace Eventify.Application.Common.Mappings;

internal sealed class CommonMappings : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Uri, string>()
            .ConstructUsing(src => src.ToString());
        
        config.NewConfig<string, Uri>()
            .ConstructUsing(src => new Uri(src));
        
        config.NewConfig<Money, decimal>()
            .ConstructUsing(src => src.Value);
        
        config.NewConfig<Language, string>()
            .ConstructUsing(src => src.Name);
        
        config.NewConfig<Quantity, int>()
            .ConstructUsing(src => src.Value);
        
        config.NewConfig<AttendeeId, Guid>()
            .ConstructUsing(src => src.Value);
        
        config.NewConfig<BookingId, Guid>()
            .ConstructUsing(src => src.Value);
        
        config.NewConfig<EventId, Guid>()
            .ConstructUsing(src => src.Value);
        
        config.NewConfig<ProducerId, Guid>()
            .ConstructUsing(src => src.Value);
        
        config.NewConfig<TicketId, Guid>()
            .ConstructUsing(src => src.Value);
    }
}