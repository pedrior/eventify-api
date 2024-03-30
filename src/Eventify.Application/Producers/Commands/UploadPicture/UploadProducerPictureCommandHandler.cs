using Eventify.Application.Common.Abstractions.Storage;
using Eventify.Application.Producers.Common.Errors;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.Producers.Commands.UploadPicture;

internal sealed class UploadProducerPictureCommandHandler(
    IUser user,
    IProducerRepository producerRepository,
    IStorageService storageService
) : ICommandHandler<UploadProducerPictureCommand, Success>
{
    public async Task<Result<Success>> Handle(UploadProducerPictureCommand command,
        CancellationToken cancellationToken)
    {
        var producer = await producerRepository.GetByUserAsync(user.Id, cancellationToken);
        if (producer is null)
        {
            throw new ApplicationException("Producer not found");
        }

        var pictureUrl = await storageService.UploadAsync(
            StorageKeys.ProducerPicture(producer.Id),
            command.Picture,
            cancellationToken);

        if (pictureUrl is null)
        {
            return ProducerErrors.PictureUploadFailed;
        }

        return await producer.SetPicture(pictureUrl)
            .ThenAsync(() => producerRepository.UpdateAsync(producer, cancellationToken));
    }
}