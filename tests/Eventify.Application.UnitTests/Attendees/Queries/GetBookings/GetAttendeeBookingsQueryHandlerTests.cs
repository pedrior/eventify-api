using Eventify.Application.Attendees.Queries.GetBookings;
using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.UnitTests.Attendees.Queries.GetBookings;

[TestSubject(typeof(GetAttendeeBookingsQueryHandler))]
public sealed class GetAttendeeBookingsQueryHandlerTests
{
    private readonly IUser user = A.Fake<IUser>();
    private readonly IAttendeeRepository attendeeRepository = A.Fake<IAttendeeRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly GetAttendeeBookingsQuery query = new();

    private readonly GetAttendeeBookingsQueryHandler sut;

    public GetAttendeeBookingsQueryHandlerTests()
    {
        sut = new GetAttendeeBookingsQueryHandler(user, attendeeRepository);
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
    public async Task Handle_WhenAttendeeExists_ShouldReturnBookingIds()
    {
        // Arrange
        var attendee = Factories.Attendee.CreateAttendee();

        A.CallTo(() => attendeeRepository.GetByUserAsync(user.Id, cancellationToken))
            .Returns(attendee);

        // Act
        var result = await sut.Handle(query, cancellationToken);

        // Assert
        result.Value.Should().BeEquivalentTo(attendee.BookingIds.Select(id => id.Value));
    }
}