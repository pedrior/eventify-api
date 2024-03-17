using Eventify.Domain.Common.Enums;
using Eventify.Domain.Common.ValueObjects;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Enums;
using Eventify.Domain.Events.Errors;
using Eventify.Domain.Events.Events;
using Eventify.Domain.Events.Services;
using Eventify.Domain.Events.ValueObjects;

namespace Eventify.Domain.UnitTests.Events;

[TestSubject(typeof(Event))]
public sealed partial class EventTests
{
    [Fact]
    public void Create_WhenCalled_ShouldCreateEventWithDraftState()
    {
        // Arrange
        // Act
        var sut = Factories.Event.CreateEvent();

        // Assert
        sut.State.Should().Be(EventState.Draft);
    }

    [Fact]
    public void Create_WhenCalled_ShouldCreateEventWithSlug()
    {
        // Arrange
        // Act
        var sut = Factories.Event.CreateEvent();

        // Assert
        sut.Slug.Should().NotBeNull();
        sut.Slug.Value.Should().StartWithEquivalentOf(
            Slug.Create(sut.Details.Name, randomSuffix: false).Value);
    }

    [Fact]
    public void Create_WhenCalled_ShouldRaiseEventCreated()
    {
        // Arrange
        // Act
        var sut = Factories.Event.CreateEvent();

        // Assert
        sut.DomainEvents.Should().ContainSingle(e => e is EventCreated);
    }

    [Fact]
    public void UpdateDetails_WhenEventIsFinished_ShouldReturnCannotModifyFinishedEvent()
    {
        // Arrange
        var sut = CreateFinishedEvent();

        var newDetails = new EventDetails(
            name: "Lorem Ipsum",
            category: EventCategory.Expo,
            language: Language.Portuguese);

        // Act
        var result = sut.UpdateDetails(newDetails);

        // Assert
        result.Should().BeError(EventErrors.CannotModifyFinishedEvent);
    }

    [Fact]
    public void UpdateDetails_WhenCalled_ShouldUpdateDetails()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var newDetails = new EventDetails(
            name: "Lorem Ipsum",
            category: EventCategory.Expo,
            language: Language.Portuguese);

        // Act
        sut.UpdateDetails(newDetails);

        // Assert
        sut.Details.Should().Be(newDetails);
    }

    [Fact]
    public void UpdateDetails_WhenDetailsIsUpdated_ShouldReturnUpdated()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var newDetails = new EventDetails(
            name: "Lorem Ipsum",
            category: EventCategory.Expo,
            language: Language.Portuguese);

        // Act
        var result = sut.UpdateDetails(newDetails);

        // Assert
        result.Should().BeValue(Result.Updated);
    }

    [Fact]
    public void UpdatePeriod_WhenEventIsFinished_ShouldReturnCannotModifyFinishedEvent()
    {
        // Arrange
        var sut = CreateFinishedEvent();

        var newPeriod = new Period(
            start: DateTimeOffset.UtcNow.AddMonths(1),
            end: DateTimeOffset.UtcNow.AddMonths(2));

        // Act
        var result = sut.UpdatePeriod(newPeriod);

        // Assert
        result.Should().BeError(EventErrors.CannotModifyFinishedEvent);
    }

    [Fact]
    public void UpdatePeriod_WhenEventIsPublished_ShouldReturnInvalidOperation()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        sut.AddTicket(Factories.Ticket.CreateTicketValue());
        sut.Publish();

        var newPeriod = new Period(
            start: DateTimeOffset.UtcNow.AddMonths(1),
            end: DateTimeOffset.UtcNow.AddMonths(2));

        // Act
        var result = sut.UpdatePeriod(newPeriod);

        // Assert
        result.Should().BeError(EventErrors.InvalidOperation(sut.State));
    }

    [Fact]
    public void UpdatePeriod_WhenEventIsNotPublished_ShouldUpdatePeriod()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var newPeriod = new Period(
            start: DateTimeOffset.UtcNow.AddMonths(1),
            end: DateTimeOffset.UtcNow.AddMonths(2));

        // Act
        sut.UpdatePeriod(newPeriod);

        // Assert
        sut.Period.Should().Be(newPeriod);
    }

    [Fact]
    public void UpdatePeriod_WhenEventIsNotPublished_ShouldReturnUpdated()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var newPeriod = new Period(
            start: DateTimeOffset.UtcNow.AddMonths(1),
            end: DateTimeOffset.UtcNow.AddMonths(2));

        // Act
        var result = sut.UpdatePeriod(newPeriod);

        // Assert
        result.Should().BeValue(Result.Updated);
    }

    [Fact]
    public void UpdateLocation_WhenEventIsFinished_ShouldReturnCannotModifyFinishedEvent()
    {
        // Arrange
        var sut = CreateFinishedEvent();

        var newLocation = new EventLocation(
            name: "Lorem Ipsum",
            address: "Lorem Ipsum Dolor Sit Amet",
            zipCode: "12345-678",
            city: "Ipsum",
            state: "Dolor",
            country: "Sit");

        // Act
        var result = sut.UpdateLocation(newLocation);

        // Assert
        result.Should().BeError(EventErrors.CannotModifyFinishedEvent);
    }

    [Fact]
    public void UpdateLocation_WhenEventIsPublished_ShouldReturnInvalidOperation()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        sut.AddTicket(Factories.Ticket.CreateTicketValue());
        sut.Publish();

        var newLocation = new EventLocation(
            name: "Lorem Ipsum",
            address: "Lorem Ipsum Dolor Sit Amet",
            zipCode: "12345-678",
            city: "Ipsum",
            state: "Dolor",
            country: "Sit");

        // Act
        var result = sut.UpdateLocation(newLocation);

        // Assert
        result.Should().BeError(EventErrors.InvalidOperation(sut.State));
    }

    [Fact]
    public void UpdateLocation_WhenEventIsNotPublished_ShouldUpdateLocation()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var newLocation = new EventLocation(
            name: "Lorem Ipsum",
            address: "Lorem Ipsum Dolor Sit Amet",
            zipCode: "12345-678",
            city: "Ipsum",
            state: "Dolor",
            country: "Sit");

        // Act
        sut.UpdateLocation(newLocation);

        // Assert
        sut.Location.Should().Be(newLocation);
    }

    [Fact]
    public void UpdateLocation_WhenEventIsNotPublished_ShouldReturnUpdated()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var newLocation = new EventLocation(
            name: "Lorem Ipsum",
            address: "Lorem Ipsum Dolor Sit Amet",
            zipCode: "12345-678",
            city: "Ipsum",
            state: "Dolor",
            country: "Sit");

        // Act
        var result = sut.UpdateLocation(newLocation);

        // Assert
        result.Should().BeValue(Result.Updated);
    }

    [Fact]
    public async Task UpdateSlugAsync_WhenEventIsFinished_ShouldReturnCannotModifyFinishedEvent()
    {
        // Arrange
        var sut = CreateFinishedEvent();

        var uniquenessChecker = A.Fake<IEventSlugUniquenessChecker>();

        var newSlug = Slug.Create("lorem-ipsum", randomSuffix: false);

        // Act
        var result = await sut.UpdateSlugAsync(newSlug, uniquenessChecker);

        // Assert
        result.Should().BeError(EventErrors.CannotModifyFinishedEvent);
    }

    [Fact]
    public async Task UpdateSlugAsync_WhenSlugIsNotUnique_ShouldReturnSlugAlreadyExists()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();

        var uniquenessChecker = A.Fake<IEventSlugUniquenessChecker>();

        var newSlug = Slug.Create("lorem-ipsum", randomSuffix: false);

        A.CallTo(() => uniquenessChecker.IsUniqueAsync(newSlug, A.Dummy<CancellationToken>()))
            .Returns(false);

        // Act
        var result = await sut.UpdateSlugAsync(newSlug, uniquenessChecker);

        // Assert
        result.Should().BeError(EventErrors.SlugAlreadyExists(newSlug));
    }

    [Fact]
    public async Task UpdateSlugAsync_WhenSlugIsUnique_ShouldUpdateSlug()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();

        var uniquenessChecker = A.Fake<IEventSlugUniquenessChecker>();

        var newSlug = Slug.Create("lorem-ipsum", randomSuffix: false);

        A.CallTo(() => uniquenessChecker.IsUniqueAsync(newSlug, A.Dummy<CancellationToken>()))
            .Returns(true);

        // Act
        await sut.UpdateSlugAsync(newSlug, uniquenessChecker);

        // Assert
        sut.Slug.Should().Be(newSlug);
    }

    [Fact]
    public async Task UpdateSlugAsync_WhenSlugIsUpdated_ShouldReturnUpdated()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();

        var uniquenessChecker = A.Fake<IEventSlugUniquenessChecker>();

        var newSlug = Slug.Create("lorem-ipsum", randomSuffix: false);

        A.CallTo(() => uniquenessChecker.IsUniqueAsync(newSlug, A.Dummy<CancellationToken>()))
            .Returns(true);

        // Act
        var result = await sut.UpdateSlugAsync(newSlug, uniquenessChecker);

        // Assert
        result.Should().BeValue(Result.Updated);
    }

    [Fact]
    public void SetPoster_WhenEventIsFinished_ShouldReturnCannotModifyFinishedEvent()
    {
        // Arrange
        var sut = CreateFinishedEvent();
        var posterUrl = new Uri("https://events.com/poster.jpg");

        // Act
        var result = sut.SetPoster(posterUrl);

        // Assert
        result.Should().BeError(EventErrors.CannotModifyFinishedEvent);
    }

    [Fact]
    public void SetPoster_WhenCalled_ShouldSetPoster()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var posterUrl = new Uri("https://events.com/poster.jpg");

        // Act
        sut.SetPoster(posterUrl);

        // Assert
        sut.PosterUrl.Should().Be(posterUrl);
    }

    [Fact]
    public void SetPoster_WhenPosterIsSet_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var posterUrl = new Uri("https://events.com/poster.jpg");

        // Act
        var result = sut.SetPoster(posterUrl);

        // Assert
        result.Should().BeValue(Result.Success);
    }

    [Fact]
    public void RemovePoster_WhenEventIsFinished_ShouldReturnCannotModifyFinishedEvent()
    {
        // Arrange
        var sut = CreateFinishedEvent();

        // Act
        var result = sut.RemovePoster();

        // Assert
        result.Should().BeError(EventErrors.CannotModifyFinishedEvent);
    }

    [Fact]
    public void RemovePoster_WhenCalled_ShouldRemovePoster()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var posterUrl = new Uri("https://events.com/poster.jpg");
        sut.SetPoster(posterUrl);

        // Act
        sut.RemovePoster();

        // Assert
        sut.PosterUrl.Should().BeNull();
    }

    [Fact]
    public void RemovePoster_WhenPosterIsRemoved_ShouldReturnDeleted()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        var posterUrl = new Uri("https://events.com/poster.jpg");
        sut.SetPoster(posterUrl);

        // Act
        var result = sut.RemovePoster();

        // Assert
        result.Should().BeValue(Result.Deleted);
    }

    [Fact]
    public void Publish_WhenTransitionIsInvalid_ShouldReturnInvalidOperation()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        sut.AddTicket(Factories.Ticket.CreateTicketValue());

        sut.Publish();

        // Act
        var result = sut.Publish();

        // Assert
        result.Should().BeError(EventErrors.InvalidOperation(sut.State));
    }

    [Fact]
    public void Publish_WhenPeriodIsPast_ShouldReturnMustBeInFuture()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent(
            period: new Period(
                start: DateTimeOffset.UtcNow,
                end: DateTimeOffset.UtcNow));

        sut.AddTicket(Factories.Ticket.CreateTicketValue());

        // Act
        var result = sut.Publish();

        // Assert
        result.Should().BeError(EventErrors.MustBeInFuture);
    }

    [Fact]
    public void Publish_WhenEventHasNoTickets_ShouldReturnMustHaveTicket()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();

        // Act
        var result = sut.Publish();

        // Assert
        result.Should().BeError(EventErrors.MustHaveTicket);
    }

    [Fact]
    public void Publish_WhenTransitionIsValid_ShouldPublishEvent()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        sut.AddTicket(Factories.Ticket.CreateTicketValue());

        // Act
        sut.Publish();

        // Assert
        sut.State.Should().Be(EventState.Published);
        sut.PublishedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Publish_WhenTransitionIsValid_ShouldRaiseEventPublished()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        sut.AddTicket(Factories.Ticket.CreateTicketValue());

        // Act
        sut.Publish();

        // Assert
        sut.DomainEvents.Should().ContainSingle(e => e is EventPublished);
    }

    [Fact]
    public void Publish_WhenTransitionIsValid_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        sut.AddTicket(Factories.Ticket.CreateTicketValue());

        // Act
        var result = sut.Publish();

        // Assert
        result.Should().BeValue(Result.Success);
    }

    [Fact]
    public void Unpublish_WhenTransitionIsInvalid_ShouldReturnInvalidOperation()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();

        // Act
        var result = sut.Unpublish();

        // Assert
        result.Should().BeError(EventErrors.InvalidOperation(sut.State));
    }

    [Fact]
    public void Unpublish_WhenTransitionIsValid_ShouldUnpublishEvent()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        sut.AddTicket(Factories.Ticket.CreateTicketValue());
        sut.Publish();

        // Act
        sut.Unpublish();

        // Assert
        sut.State.Should().Be(EventState.Draft);
        sut.PublishedAt.Should().BeNull();
    }

    [Fact]
    public void Unpublish_WhenTransitionIsValid_ShouldRaiseEventUnpublished()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        sut.AddTicket(Factories.Ticket.CreateTicketValue());
        sut.Publish();

        // Act
        sut.Unpublish();

        // Assert
        sut.DomainEvents.Should().ContainSingle(e => e is EventUnpublished);
    }

    [Fact]
    public void Unpublish_WhenTransitionIsValid_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Event.CreateEvent();
        sut.AddTicket(Factories.Ticket.CreateTicketValue());
        sut.Publish();

        // Act
        var result = sut.Unpublish();

        // Assert
        result.Should().BeValue(Result.Success);
    }

    private static Event CreateFinishedEvent()
    {
        var sut = Factories.Event.CreateEvent();

        sut.AddTicket(Factories.Ticket.CreateTicketValue());
        sut.UpdatePeriod(new Period(
            start: DateTimeOffset.UtcNow.AddMilliseconds(100),
            end: DateTimeOffset.UtcNow.AddMilliseconds(100)));

        sut.Publish();

        Thread.Sleep(200);

        return sut;
    }
}