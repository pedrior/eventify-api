namespace Eventify.Application.Producers.Commands.RemovePicture;

public sealed record RemoveProducerPictureCommand : ICommand<Success>, ITransactional;