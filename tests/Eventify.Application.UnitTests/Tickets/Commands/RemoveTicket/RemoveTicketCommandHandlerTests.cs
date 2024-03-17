using Eventify.Application.Tickets.Commands.RemoveTicket;
using Eventify.Application.Tickets.Common.Errors;
using Eventify.Domain.Events;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;

namespace Eventify.Application.UnitTests.Tickets.Commands.RemoveTicket;

[TestSubject(typeof(RemoveTicketCommandHandler))]
public sealed class RemoveTicketCommandHandlerTests
{
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();
    private readonly ITicketRepository ticketRepository = A.Fake<ITicketRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly Event @event = Factories.Event.CreateEvent();
    private readonly Ticket ticket = Factories.Ticket.CreateTicketValue();

    private readonly RemoveTicketCommand command = new()
    {
        TicketId = Constants.Ticket.TicketId.Value
    };

    private readonly RemoveTicketCommandHandler sut;

    public RemoveTicketCommandHandlerTests()
    {
        sut = new RemoveTicketCommandHandler(eventRepository, ticketRepository);

        A.CallTo(() => ticketRepository.GetAsync(command.TicketId, cancellationToken))
            .Returns(ticket);

        A.CallTo(() => eventRepository.GetAsync(ticket.EventId, cancellationToken))
            .Returns(@event);
    }

    [Fact]
    public async Task Handle_WhenTicketDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        A.CallTo(() => ticketRepository.GetAsync(command.TicketId, cancellationToken))
            .Returns(null as Ticket);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeError(TicketErrors.NotFound(command.TicketId));
    }

    [Fact]
    public async Task Handle_WhenTicketEventDoesNotExist_ShouldThrowApplicationException()
    {
        // Arrange
        A.CallTo(() => eventRepository.GetAsync(ticket.EventId, cancellationToken))
            .Returns(null as Event);

        // Act
        var act = () => sut.Handle(command, cancellationToken);

        // Assert
        await act.Should().ThrowAsync<ApplicationException>()
            .WithMessage($"Event {ticket.EventId} not found");
    }

    [Fact]
    public async Task Handle_WhenTicketRemovalSucceeds_ShouldReturnDeleted()
    {
        // Arrange
        @event.AddTicket(ticket);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeValue(Result.Deleted);
    }

    [Fact]
    public async Task Handle_WhenTicketRemovalSucceeds_ShouldUpdateEvent()
    {
        // Arrange
        @event.AddTicket(ticket);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => eventRepository.UpdateAsync(@event, cancellationToken))
            .MustHaveHappened();
    }

    [Fact]
    public async Task Handle_WhenTicketRemovalFails_ShouldReturnError()
    {
        // Arrange
        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeError();
    }
}