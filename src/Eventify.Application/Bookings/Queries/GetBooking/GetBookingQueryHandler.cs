using Eventify.Application.Bookings.Common.Errors;
using Eventify.Contracts.Bookings.Responses;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Tickets.Repository;

namespace Eventify.Application.Bookings.Queries.GetBooking;

internal sealed class GetBookingQueryHandler(
    IEventRepository eventRepository,
    IBookingRepository bookingRepository,
    ITicketRepository ticketRepository
) : IQueryHandler<GetBookingQuery, BookingResponse>
{
    public async Task<Result<BookingResponse>> Handle(GetBookingQuery query,
        CancellationToken cancellationToken)
    {
        var booking = await bookingRepository.GetAsync(query.BookingId, cancellationToken);
        if (booking is null)
        {
            return BookingErrors.NotFound(query.BookingId);
        }

        var @event = await eventRepository.GetAsync(booking.EventId, cancellationToken);
        if (@event is null)
        {
            throw new ApplicationException($"Event {booking.EventId} not found");
        }

        var ticket = await ticketRepository.GetAsync(booking.TicketId, cancellationToken);
        if (ticket is null)
        {
            throw new ApplicationException($"Ticket {booking.TicketId} not found");
        }

        return booking.Adapt<BookingResponse>() with
        {
            Event = new BookingEventResponse
            {
                Id = @event.Id.Value,
                Name = @event.Details.Name,
                Location = @event.Location.ToString(),
                Start = @event.Period.Start,
                End = @event.Period.End
            },
            Ticket = new BookingTicketResponse
            {
                Id = ticket.Id.Value,
                Name = ticket.Name,
                Price = ticket.Price.Value
            }
        };
    }
}