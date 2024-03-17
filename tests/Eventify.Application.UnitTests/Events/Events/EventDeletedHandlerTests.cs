using Eventify.Application.Events.Events;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Producers.Events;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Events.Events;

[TestSubject(typeof(EventDeletedHandler))]
public sealed class EventDeletedHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
    private readonly ITicketRepository ticketRepository = A.Fake<ITicketRepository>();
    private readonly ILogger<EventDeletedHandler> logger = A.Fake<ILogger<EventDeletedHandler>>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly Event @event = Factories.Event.CreateEvent();

    private readonly EventDeleted e;
    private readonly EventDeletedHandler sut;

    public EventDeletedHandlerTests()
    {
        e = new EventDeleted(@event);
        sut = new EventDeletedHandler(eventRepository, ticketRepository, logger);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldRemoveAllEventTickets()
    {
        // Arrange
        A.CallTo(() => ticketRepository.ListByEventAsync(@event.Id, cancellationToken))
            .Returns(
            [
                Factories.Ticket.CreateTicketValue(),
                Factories.Ticket.CreateTicketValue(),
                Factories.Ticket.CreateTicketValue()
            ]);

        // Act
        await sut.Handle(e, cancellationToken);

        // Assert
        A.CallTo(() => ticketRepository.RemoveAsync(A<Ticket>._, cancellationToken))
            .MustHaveHappened(3, Times.Exactly);
    }

    [Fact]
    public async Task Handle_WhenCalled_ShouldRemoveEvent()
    {
        // Act
        await sut.Handle(e, cancellationToken);

        // Assert
        A.CallTo(() => eventRepository.RemoveAsync(@event, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }
}