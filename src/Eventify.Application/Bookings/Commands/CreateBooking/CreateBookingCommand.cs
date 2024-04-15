using Eventify.Application.Common.Abstractions.Persistence;
using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Contracts.Bookings.Responses;

namespace Eventify.Application.Bookings.Commands.CreateBooking;

public sealed record CreateBookingCommand : ICommand<BookingResponse>, ITransactional
{
    public required Guid TicketId { get; init; }
}