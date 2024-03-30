using Eventify.Application.Common.Mappings;
using Eventify.Application.Events.Commands.CreateEvent;
using Eventify.Application.Events.Common.Mappings;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.UnitTests.Events.Commands.CreateEvent;

[TestSubject(typeof(CreateEventCommandHandler))]
public sealed class CreateEventCommandHandlerTest
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IProducerRepository producerRepository = A.Fake<IProducerRepository>();
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly CreateEventCommand command = new()
    {
        Name = ".NET Conference",
        Category = "conference",
        Language = "en",
        Description = "This time at .NET Conference, we will be discussing the latest trends in .NET development.",
        PeriodStart = DateTimeOffset.UtcNow.AddDays(2),
        PeriodEnd = DateTimeOffset.UtcNow.AddDays(10),
        LocationName = "Centro de Convenções de João Pessoa",
        LocationAddress = "Rodovia PB-008, Km 5 s/n Polo Turístico - Cabo Branco",
        LocationZipCode = "58000000",
        LocationCity = "João Pessoa",
        LocationState = "PB",
        LocationCountry = "BR"
    };

    private readonly CreateEventCommandHandler sut;

    public CreateEventCommandHandlerTest()
    {
        TypeAdapterConfig.GlobalSettings.Apply(new CommonMappings());
        TypeAdapterConfig.GlobalSettings.Apply(new EventMappings());

        sut = new CreateEventCommandHandler(user, producerRepository, eventRepository);

        A.CallTo(() => user.Id)
            .Returns(Constants.UserId);
    }

    [Fact]
    public async Task Handle_WhenProducerDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(null as Producer);

        // Act
        var act = () => sut.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage("Producer not found");
    }

    [Fact]
    public async Task Handle_WhenCommandIsValid_ShouldCreateEvent()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => eventRepository.AddAsync(A<Event>.That.Matches(e =>
                    e.ProducerId == producer.Id),
                cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenCommandIsValid_ShouldReturnEventEditorResponse()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSuccess();
    }
}