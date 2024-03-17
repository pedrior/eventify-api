using Eventify.Application.Attendees.Events;
using Eventify.Application.Common.Abstractions.Identity;
using Eventify.Application.Common.Security;
using Eventify.Domain.Attendees.Events;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Attendees.Events;

[TestSubject(typeof(AttendeeCreatedHandler))]
public sealed class AttendeeCreatedHandlerTests
{
    private readonly IIdentityService identityService = A.Fake<IIdentityService>();
    private readonly ILogger<AttendeeCreatedHandler> logger = A.Fake<ILogger<AttendeeCreatedHandler>>();
    
    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly AttendeeCreatedHandler sut;
    
    public AttendeeCreatedHandlerTests()
    {
        sut = new AttendeeCreatedHandler(identityService, logger);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldAddUserToAttendeeRole()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee();
        
        // Act
        await sut.Handle(new AttendeeCreated(attendee), cancellationToken);
        
        // Assert
        A.CallTo(() => identityService.AddUserToRoleAsync(attendee.UserId, Roles.Attendee))
            .MustHaveHappenedOnceExactly();
    }
}