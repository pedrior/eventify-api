using Eventify.Application.Common.Mappings;
using Eventify.Application.Producers.Common.Mappings;
using Eventify.Application.Producers.Queries.GetProfile;
using Eventify.Contracts.Producers.Responses;
using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.UnitTests.Producers.Queries.GetProfile;

[TestSubject(typeof(GetProducerProfileQueryHandler))]
public sealed class GetProducerProfileQueryHandlerTests
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IProducerRepository producerRepository = A.Fake<IProducerRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly GetProducerProfileQuery query = new();

    private readonly GetProducerProfileQueryHandler sut;

    public GetProducerProfileQueryHandlerTests()
    {
        TypeAdapterConfig.GlobalSettings.Apply(new CommonMappings());
        TypeAdapterConfig.GlobalSettings.Apply(new ProducerMappings());

        sut = new GetProducerProfileQueryHandler(user, producerRepository);

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
        var act = () => sut.Handle(query, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage("Producer not found");
    }

    [Fact]
    public async Task Handle_WhenProducerExists_ShouldReturnProfileResponse()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();

        A.CallTo(() => producerRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(producer);

        // Act
        var result = await sut.Handle(query, cancellationToken);

        // Assert
        result.Value.Should().BeEquivalentTo(producer.Adapt<ProducerProfileResponse>());
    }
}