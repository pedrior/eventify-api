using Eventify.Application.Common.Abstractions.Identity;
using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Domain.Attendees;
using Eventify.Domain.Attendees.Repository;
using Eventify.Domain.Attendees.ValueObjects;
using Eventify.Domain.Users.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Eventify.Application.Account.Commands.Register;

internal sealed class RegisterCommandHandler(
    IIdentityService identityService,
    IAttendeeRepository attendeeRepository,
    ILogger<RegisterCommandHandler> logger
) : ICommandHandler<RegisterCommand, Success>
{
    public async Task<Result<Success>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await identityService.CreateUserAsync(command.Email, command.Password);
        return await result.ThenAsync(userId =>
        {
            logger.LogInformation("New account created for user {UserId}", userId);
            
            return CreateAttendeeAsync(
                userId,
                command.Email,
                command.GivenName,
                command.FamilyName,
                command.BirthDate,
                command.PhoneNumber,
                cancellationToken);
        });
    }

    private async Task<Success> CreateAttendeeAsync(
        string userId,
        string email,
        string givenName,
        string familyName,
        DateOnly? birthDate,
        string? phoneNumber = null,
        CancellationToken cancellationToken = default)
    {
        var attendee = Attendee.Create(
            attendeeId: AttendeeId.New(),
            userId: new UserId(userId),
            details: new AttendeeDetails(givenName, familyName, birthDate),
            contact: new AttendeeContact(email, phoneNumber));

        await attendeeRepository.AddAsync(attendee, cancellationToken);
        
        logger.LogInformation("Attendee user created for user {UserId}", userId);

        return Success.Value;
    }
}