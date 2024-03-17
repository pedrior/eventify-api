using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Errors;
using Eventify.Domain.Producers.Events;

namespace Eventify.Domain.UnitTests.Producers;

[TestSubject(typeof(Producer))]
public sealed partial class ProducerTests
{
    [Fact]
    public void AddEvent_WhenEventIsAlreadyAdded_ShouldReturnEventAlreadyAdded()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        sut.AddEvent(@event);

        // Act
        var result = sut.AddEvent(@event);

        // Assert
        result.Should().BeError(ProducerErrors.EventAlreadyAdded(@event.Id));
    }

    [Fact]
    public void AddEvent_WhenEventDoesNotExist_ShouldAddEvent()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        // Act
        sut.AddEvent(@event);

        // Assert
        sut.EventIds.Should().Contain(@event.Id);
    }

    [Fact]
    public void AddEvent_WhenEventIsAdded_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        // Act
        var result = sut.AddEvent(@event);

        // Assert
        result.Should().BeValue(Result.Success);
    }

    [Fact]
    public void DeleteEvent_WhenEventDoesNotExist_ShouldReturnEventNotFound()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        // Act
        var result = sut.DeleteEvent(@event);

        // Assert
        result.Should().BeError(ProducerErrors.EventNotFound(@event.Id));
    }

    [Fact]
    public void DeleteEvent_WhenEventIsPublished_ShouldReturnCannotDeleteOngoingEvent()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        @event.AddTicket(Factories.Ticket.CreateTicketValue());
        @event.Publish();

        sut.AddEvent(@event);

        // Act
        var result = sut.DeleteEvent(@event);

        // Assert
        result.Should().BeError(ProducerErrors.CannotDeleteOngoingEvent(@event.Id));
    }

    [Fact]
    public void DeleteEvent_WhenEventCanBeDeleted_ShouldRemoveEvent()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        sut.AddEvent(@event);

        // Act
        sut.DeleteEvent(@event);

        // Assert
        sut.EventIds.Should().NotContain(@event.Id);
    }

    [Fact]
    public void DeleteEvent_WhenEventIsDeleted_ShouldAddEventToDeletedEvent()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        sut.AddEvent(@event);

        // Act
        sut.DeleteEvent(@event);

        // Assert
        sut.DeletedEventIds.Should().Contain(@event.Id);
    }

    [Fact]
    public void DeleteEvent_WhenEvenIsDeleted_ShouldRaiseEventDeleted()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        sut.AddEvent(@event);

        // Act
        sut.DeleteEvent(@event);

        // Assert
        sut.DomainEvents.Should().ContainSingle(x => x is EventDeleted);
    }

    [Fact]
    public void DeleteEvent_WhenEventIsDeleted_ShouldReturnDeleted()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        sut.AddEvent(@event);

        // Act
        var result = sut.DeleteEvent(@event);

        // Assert
        result.Should().BeValue(Result.Deleted);
    }

    [Fact]
    public void PublishEvent_WhenEventDoesNotExist_ShouldReturnEventNotFound()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        // Act
        var result = sut.PublishEvent(@event);

        // Assert
        result.Should().BeError(ProducerErrors.EventNotFound(@event.Id));
    }
    
    [Fact]
    public void PublishEvent_WhenPublishEventFails_ShouldReturnError()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        // Act
        var result = sut.PublishEvent(@event);

        // Assert
        result.Should().BeError();
    }

    [Fact]
    public void PublishEvent_WhenPublishEventSucceeds_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        @event.AddTicket(Factories.Ticket.CreateTicketValue());

        // Act
        var result = sut.PublishEvent(@event);

        // Assert
        result.Should().BeError();
    }

    [Fact]
    public void UnpublishEvent_WhenEventDoesNotExist_ShouldReturnEventNotFound()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        // Act
        var result = sut.UnpublishEvent(@event);

        // Assert
        result.Should().BeError(ProducerErrors.EventNotFound(@event.Id));
    }

    [Fact]
    public void UnpublishEvent_WhenUnpublishEventFails_ShouldReturnError()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        // Act
        var result = sut.UnpublishEvent(@event);

        // Assert
        result.Should().BeError();
    }

    [Fact]
    public void UnpublishEvent_WhenUnpublishEventSucceeds_ShouldReturnSuccess()
    {
        // Arrange
        var sut = Factories.Producer.CreateProducer();
        var @event = Factories.Event.CreateEvent();

        @event.AddTicket(Factories.Ticket.CreateTicketValue());
        @event.Publish();

        sut.AddEvent(@event);

        // Act
        var result = sut.UnpublishEvent(@event);

        // Assert
        result.Should().BeValue(Result.Success);
    }
}