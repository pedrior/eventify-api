using Eventify.Contracts.Events.Responses;
using Eventify.Domain.Common.Enums;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Enums;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Events.ValueObjects;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Users;
using EventId = Eventify.Domain.Events.ValueObjects.EventId;

namespace Eventify.Application.Events.Commands.CreateEvent;

internal sealed class CreateEventCommandHandler(
    IUser user,
    IProducerRepository producerRepository,
    IEventRepository eventRepository
) : ICommandHandler<CreateEventCommand, EventEditResponse>
{
    public async Task<ErrorOr<EventEditResponse>> Handle(CreateEventCommand command,
        CancellationToken cancellationToken)
    {
        var producer = await producerRepository.GetByUserAsync(user.Id, cancellationToken);
        if (producer is null)
        {
            throw new ApplicationException("Producer not found");
        }

        var details = new EventDetails(
            name: command.Name,
            description: command.Description,
            category: EventCategory.FromName(command.Category),
            language: Language.FromName(command.Language));

        var location = new EventLocation(
            name: command.LocationName,
            address: command.LocationAddress,
            zipCode: command.LocationZipCode,
            city: command.LocationCity,
            state: command.LocationState,
            country: command.LocationCountry);

        var period = new Period(
            start: command.PeriodStart,
            end: command.PeriodEnd);

        var @event = Event.Create(EventId.New(), producer.Id, details, location, period);

        await eventRepository.AddAsync(@event, cancellationToken);

        return @event.Adapt<EventEditResponse>();
    }
}