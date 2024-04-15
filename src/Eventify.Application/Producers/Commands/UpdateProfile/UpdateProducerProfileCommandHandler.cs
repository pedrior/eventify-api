using Eventify.Application.Common.Abstractions.Requests;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Producers.ValueObjects;
using Eventify.Domain.Users;

namespace Eventify.Application.Producers.Commands.UpdateProfile;

internal sealed class UpdateProducerProfileCommandHandler(
    IUser user,
    IProducerRepository producerRepository
) : ICommandHandler<UpdateProducerProfileCommand, Success>
{
    public async Task<Result<Success>> Handle(UpdateProducerProfileCommand command,
        CancellationToken cancellationToken)
    {
        var producer = await producerRepository.GetByUserAsync(user.Id, cancellationToken);
        if (producer is null)
        {
            throw new ApplicationException("Producer not found");
        }

        var details = new ProducerDetails(command.Name, command.Bio);
        var contact = new ProducerContact(command.Email, command.PhoneNumber);

        return await producer.UpdateProfile(details, contact, command.WebsiteUrl)
            .ThenAsync(() => producerRepository.UpdateAsync(producer, cancellationToken));
    }
}