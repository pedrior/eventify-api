using Eventify.Application.Producers.Common.Errors;
using Eventify.Domain.Producers;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Producers.ValueObjects;
using Eventify.Domain.Users;

namespace Eventify.Application.Producers.Commands.CreateProfile;

internal sealed class CreateProducerProfileCommandHandler(
    IUser user,
    IProducerRepository producerRepository
) : ICommandHandler<CreateProducerProfileCommand, Created>
{
    public async Task<ErrorOr<Created>> Handle(CreateProducerProfileCommand command,
        CancellationToken cancellationToken)
    {
        if (await producerRepository.ExistsByUserAsync(user.Id, cancellationToken))
        {
            return ProducerErrors.ProfileAlreadyCreated;
        }

        var details = new ProducerDetails(command.Name, command.Bio);
        var contact = new ProducerContact(command.Email, command.PhoneNumber);

        var producer = Producer.Create(
            ProducerId.New(),
            user.Id,
            details,
            contact,
            websiteUrl: command.WebsiteUrl);

        await producerRepository.AddAsync(producer, cancellationToken);

        return Result.Created;
    }
}