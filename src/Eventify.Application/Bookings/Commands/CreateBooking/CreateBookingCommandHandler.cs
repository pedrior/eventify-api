using Eventify.Application.Bookings.Common.Errors;
using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Contracts.Bookings.Responses;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Bookings.Repository;
using Eventify.Domain.Bookings.Services;
using Eventify.Domain.Events.Repository;
using Eventify.Domain.Tickets.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.Bookings.Commands.CreateBooking;

internal sealed class CreateBookingCommandHandler(
    IUser user,
    IAttendeeRepository attendeeRepository,
    IBookingRepository bookingRepository,
    IEventRepository eventRepository,
    ITicketRepository ticketRepository,
    IBookingService bookingService
) : ICommandHandler<CreateBookingCommand, BookingResponse>
{
    public async Task<Result<BookingResponse>> Handle(CreateBookingCommand command,
        CancellationToken cancellationToken)
    {
        var ticket = await ticketRepository.GetAsync(command.TicketId, cancellationToken);
        if (ticket is null)
        {
            return BookingErrors.TicketNotFound(command.TicketId);
        }

        var @event = await eventRepository.GetAsync(ticket.EventId, cancellationToken);
        if (@event is null)
        {
            throw new ApplicationException($"Event {ticket.EventId} not found");
        }

        var attendee = await attendeeRepository.GetByUserAsync(user.Id, cancellationToken);
        if (attendee is null)
        {
            throw new ApplicationException("Attendee not found");
        }

        var existingTicketBookings = await bookingRepository.ListByTicketAsync(command.TicketId,
            cancellationToken);

        var result = bookingService.PlaceBooking(@event, ticket, attendee, existingTicketBookings);
        return await result.ThenAsync(async booking =>
        {
            await bookingRepository.AddAsync(booking, cancellationToken);
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
        });
    }
}