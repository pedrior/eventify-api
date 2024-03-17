using Eventify.Application.Common.Abstractions.Storage;
using Eventify.Application.Producers.Common.Errors;
using Eventify.Domain.Producers.Repository;
using Eventify.Domain.Users;

namespace Eventify.Application.Producers.Commands.RemovePicture;

internal sealed class RemoveProducerPictureCommandHandler(
    IUser user,
    IProducerRepository producerRepository,
    IStorageService storageService
) : ICommandHandler<RemoveProducerPictureCommand, Deleted>
{
    public async Task<ErrorOr<Deleted>> Handle(RemoveProducerPictureCommand command,
        CancellationToken cancellationToken)
    {
        var producer = await producerRepository.GetByUserAsync(user.Id, cancellationToken);
        if (producer is null)
        {
            throw new ApplicationException("Producer not found");
        }

        if (producer.PictureUrl is null)
        {
            return Result.Deleted;
        }

        var pictureKey = StorageKeys.ProducerPicture(producer.Id);
        if (!await storageService.DeleteAsync(pictureKey, cancellationToken))
        {
            return ProducerErrors.PictureDeletionFailed;
        }

        return await producer.RemovePicture()
            .ThenAsync(_ => producerRepository.UpdateAsync(producer, cancellationToken));
    }
}