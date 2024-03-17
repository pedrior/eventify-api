using Eventify.Application.Common.Mappings;
using Eventify.Application.Tickets.Common.Errors;
using Eventify.Application.Tickets.Queries.GetTicket;
using Eventify.Contracts.Tickets.Responses;
using Eventify.Domain.Tickets;
using Eventify.Domain.Tickets.Repository;

namespace Eventify.Application.UnitTests.Tickets.Queries.GetTicket;

[TestSubject(typeof(GetTicketQueryHandler))]
public sealed class GetTicketQueryHandlerTests
{
    private readonly ITicketRepository ticketRepository = A.Fake<ITicketRepository>();

    private readonly CancellationToken cancellationToken = A.Dummy<CancellationToken>();

    private readonly GetTicketQuery query = new()
    {
        TicketId = Guid.NewGuid()
    };

    private readonly GetTicketQueryHandler sut;

    public GetTicketQueryHandlerTests()
    {
        TypeAdapterConfig.GlobalSettings.Apply(new CommonMappings());

        sut = new GetTicketQueryHandler(ticketRepository);
    }

    [Fact]
    public async Task Handle_WhenTicketDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        A.CallTo(() => ticketRepository.GetAsync(query.TicketId, cancellationToken))
            .Returns(null as Ticket);

        // Act
        var result = await sut.Handle(query, cancellationToken);

        // Assert
        result.Should().BeError(TicketErrors.NotFound(query.TicketId));
    }

    [Fact]
    public async Task Handle_WhenTicketExist_ShouldReturnTicketResponse()
    {
        // Arrange
        var ticket = Factories.Ticket.CreateTicketValue();

        A.CallTo(() => ticketRepository.GetAsync(query.TicketId, cancellationToken))
            .Returns(ticket);

        // Act
        var result = await sut.Handle(query, cancellationToken);

        // Assert
        result.Should().BeValue(ticket.Adapt<TicketResponse>());
    }
}