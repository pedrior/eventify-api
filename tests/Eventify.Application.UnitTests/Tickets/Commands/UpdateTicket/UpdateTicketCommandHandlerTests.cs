using Eventify.Application.Tickets.Commands.UpdateTicket;
using Eventify.Application.Tickets.Common.Errors;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;

namespace Eventify.Application.UnitTests.Tickets.Commands.UpdateTicket;

[TestSubject(typeof(UpdateTicketCommandHandler))]
public sealed class UpdateTicketCommandHandlerTests
{
    private readonly ITicketRepository ticketRepository = A.Fake<ITicketRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly Ticket ticket = Factories.Ticket.CreateTicketValue();

    private readonly UpdateTicketCommand command = new()
    {
        TicketId = Constants.Ticket.TicketId.Value,
        Name = "Vip 2",
        Description = null,
        Price = 19.79m,
        Quantity = 40,
        QuantityPerSale = 1,
        SaleStart = null,
        SaleEnd = null
    };

    private readonly UpdateTicketCommandHandler sut;

    public UpdateTicketCommandHandlerTests()
    {
        sut = new UpdateTicketCommandHandler(ticketRepository);

        A.CallTo(() => ticketRepository.GetAsync(command.TicketId, cancellationToken))
            .Returns(ticket);
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
    public async Task Handle_WhenTicketUpdateSucceeds_ShouldReturnUpdated()
    {
        // Arrange
        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeValue(Result.Updated);
    }

    [Fact]
    public async Task Handle_WhenTicketUpdateSucceeds_ShouldUpdateTicket()
    {
        // Arrange
        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => ticketRepository.UpdateAsync(ticket, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenTicketUpdateFails_ShouldReturnError()
    {
        // Arrange
        // Act
        var result = await sut.Handle(command with
        {
            QuantityPerSale = 0 // Invalid quantity per sale
        }, cancellationToken);

        // Assert
        result.Should().BeError();
    }
}