using Eventify.Application.Common.Abstractions.Identity;
using Eventify.Application.Common.Security;
using Eventify.Application.Producers.Events;
using Eventify.Domain.Producers.Events;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Producers.Events;

[TestSubject(typeof(ProducerCreatedHandler))]
public sealed class ProducerCreatedHandlerTests
{
    private readonly IIdentityService identityService = A.Fake<IIdentityService>();
    private readonly ILogger<ProducerCreatedHandler> logger = A.Fake<ILogger<ProducerCreatedHandler>>();
    
    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly ProducerCreatedHandler sut;
    
    public ProducerCreatedHandlerTests()
    {
        sut = new ProducerCreatedHandler(identityService, logger);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldAddUserToProducerRole()
    {
        // Arrange
        var producer = Factories.Producer.CreateProducer();
        
        // Act
        await sut.Handle(new ProducerCreated(producer), cancellationToken);
        
        // Assert
        A.CallTo(() => identityService.AddUserToRoleAsync(producer.UserId, Roles.Producer))
            .MustHaveHappenedOnceExactly();
    }
}