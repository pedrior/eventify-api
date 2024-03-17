using Eventify.Application.Attendees.Common.Mappings;
using Eventify.Application.Attendees.Queries.GetProfile;
using Eventify.Application.Common.Mappings;
using Eventify.Contracts.Attendees.Responses;
using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.UnitTests.Attendees.Queries.GetProfile;

[TestSubject(typeof(GetAttendeeProfileQueryHandler))]
public sealed class GetAttendeeProfileQueryHandlerTests
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IAttendeeRepository attendeeRepository = A.Fake<IAttendeeRepository>();
    
    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly GetAttendeeProfileQuery query = new();

    private readonly GetAttendeeProfileQueryHandler sut;

    public GetAttendeeProfileQueryHandlerTests()
    {
        sut = new GetAttendeeProfileQueryHandler(user, attendeeRepository);

        A.CallTo(() => user.Id)
            .Returns(Constants.UserId);

        TypeAdapterConfig.GlobalSettings.Apply(new CommonMappings());
        TypeAdapterConfig.GlobalSettings.Apply(new AttendeeMappings());
    }

    [Fact]
    public async Task Handle_WhenAttendeeDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(null as Attendee);

        // Act
        var act = () => sut.Handle(query, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ApplicationException>()
            .WithMessage("Attendee not found");
    }

    [Fact]
    public async Task Handle_WhenAttendeeExists_ShouldReturnProfileResponse()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee();

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

        // Act
        var result = await sut.Handle(query, cancellationToken);

        // Assert
        result.Value.Should().BeEquivalentTo(attendee.Adapt<AttendeeProfileResponse>());
    }
}