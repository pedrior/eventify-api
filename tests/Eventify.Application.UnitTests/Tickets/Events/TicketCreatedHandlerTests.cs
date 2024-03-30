using Eventify.Application.Tickets.Events;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Events;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.UnitTests.Tickets.Events;

[TestSubject(typeof(TicketCreatedHandler))]
public sealed class TicketCreatedHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
    private readonly ILogger<TicketCreatedHandler> logger = A.Fake<ILogger<TicketCreatedHandler>>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly Ticket ticket = Factories.Ticket.CreateTicketValue();
    private readonly Event @event = Factories.Event.CreateEvent();

    private readonly TicketCreated e;
    private readonly TicketCreatedHandler sut;

    public TicketCreatedHandlerTests()
    {
        e = new TicketCreated(ticket);
        sut = new TicketCreatedHandler(eventRepository, logger);

        A.CallTo(() => eventRepository.GetAsync(ticket.EventId, cancellationToken))
            .Returns(@event);
    }

    [Fact]
    public async Task Handle_WhenTicketEventDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => eventRepository.GetAsync(ticket.EventId, cancellationToken))
            .Returns(null as Event);

        // Act
        var act = () => sut.Handle(e, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ApplicationException>()
            .WithMessage($"Event {ticket.EventId} not found");
    }

    [Fact]
    public async Task Handle_WhenAddingTicketToEventFails_ShouldThrowResultException()
    {
        // Arrange
        @event.AddTicket(ticket); // Duplicate ticket

        // Act
        var act = () => sut.Handle(e, cancellationToken);

        // Assert
        await act.Should().ThrowExactlyAsync<ResultException>();
    }

    [Fact]
    public async Task Handle_WhenAddingTicketToEventSucceeds_ShouldUpdateEvent()
    {
        // Act
        await sut.Handle(e, cancellationToken);

        // Assert
        A.CallTo(() => eventRepository.UpdateAsync(@event, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }
}