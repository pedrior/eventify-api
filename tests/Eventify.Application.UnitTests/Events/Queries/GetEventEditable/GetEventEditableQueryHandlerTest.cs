using Eventify.Application.Common.Mappings;
using Eventify.Application.Events.Common.Errors;
using Eventify.Application.Events.Common.Mappings;
using Eventify.Application.Events.Queries.GetEventEditable;
using Eventify.Contracts.Events.Responses;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;

namespace Eventify.Application.UnitTests.Events.Queries.GetEventEditable;

[TestSubject(typeof(GetEventEditableQueryHandler))]
public sealed class GetEventEditableQueryHandlerTest
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly Event @event = Factories.Event.CreateEvent();

    private readonly GetEventEditableQuery query = new()
    {
        EventId = Constants.Event.EventId.Value
    };

    private readonly GetEventEditableQueryHandler sut;

    public GetEventEditableQueryHandlerTest()
    {
        TypeAdapterConfig.GlobalSettings.Apply(new CommonMappings());
        TypeAdapterConfig.GlobalSettings.Apply(new EventMappings());

        sut = new GetEventEditableQueryHandler(eventRepository);

        A.CallTo(() => eventRepository.GetAsync(query.EventId, cancellationToken))
            .Returns(@event);
    }

    [Fact]
    public async Task Handle_WhenEventDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        A.CallTo(() => eventRepository.GetAsync(query.EventId, cancellationToken))
            .Returns(null as Event);

        // Act
        var result = await sut.Handle(query, cancellationToken);

        // Assert
        result.Should().BeFailure(EventErrors.NotFound(query.EventId));
    }

    [Fact]
    public async Task Handle_WhenEventExists_ShouldReturnEventEditResponse()
    {
        // Act
        var result = await sut.Handle(query, cancellationToken);

        // Assert
        result.Should().BeSuccess()
            .Which.Value.Should().BeEquivalentTo(@event.Adapt<EventEditResponse>());
    }
}