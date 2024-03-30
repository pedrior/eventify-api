using Eventify.Application.Common.Mappings;
using Eventify.Application.Tickets.Commands.CreateTicket;
using Eventify.Application.Tickets.Common.Errors;
using Eventify.Contracts.Tickets.Responses;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;

namespace Eventify.Application.UnitTests.Tickets.Commands.CreateTicket;

[TestSubject(typeof(CreateTicketCommandHandler))]
public sealed class CreateTicketCommandHandlerTests
{
    private readonly ITicketRepository ticketRepository = A.Fake<ITicketRepository>();
    private readonly IEventRepository eventRepository = A.Fake<IEventRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly CreateTicketCommand command = new()
    {
        EventId = Constants.Event.EventId.Value,
        Name = "Vip",
        Description = "Vip ticket",
        Price = 30m,
        Quantity = 25,
        QuantityPerSale = 2,
        SaleStart = DateTimeOffset.UtcNow,
        SaleEnd = DateTimeOffset.UtcNow.AddDays(5)
    };

    private readonly CreateTicketCommandHandler sut;

    public CreateTicketCommandHandlerTests()
    {
        TypeAdapterConfig.GlobalSettings.Apply(new CommonMappings());

        sut = new CreateTicketCommandHandler(ticketRepository, eventRepository);
    }

    [Fact]
    public async Task Handle_WhenEventDoesNotExist_ShouldReturnEventNotFound()
    {
        // Arrange
        A.CallTo(() => eventRepository.ExistsAsync(command.EventId, cancellationToken))
            .Returns(false);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeFailure(TicketErrors.EventNotFound(command.EventId));
    }

    [Fact]
    public async Task Handle_WhenTicketCreationFails_ShouldReturnError()
    {
        // Arrange
        // Act
        var result = await sut.Handle(command with
        {
            QuantityPerSale = 0
        }, cancellationToken);

        // Assert
        result.Should().BeFailure();
    }

    [Fact]
    public async Task Handle_WhenTicketCreationSucceeds_ShouldAddTicketToRepository()
    {
        // Arrange
        A.CallTo(() => eventRepository.ExistsAsync(command.EventId, cancellationToken))
            .Returns(true);

        // Act
        await sut.Handle(command, cancellationToken);

        // Assert
        A.CallTo(() => ticketRepository.AddAsync(A<Ticket>._, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task Handle_WhenTicketCreationSucceeds_ShouldReturnTicketResponse()
    {
        // Arrange
        A.CallTo(() => eventRepository.ExistsAsync(command.EventId, cancellationToken))
            .Returns(true);

        // Act
        var result = await sut.Handle(command, cancellationToken);

        // Assert
        result.Should().BeSuccess()
            .Which.Value.Should().BeOfType<TicketResponse>();
    }
}