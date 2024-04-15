using Eventify.Application.Common.Abstractions.Persistence;
using Eventify.Application.Common.Abstractions.Requests;

namespace Eventify.Application.Producers.Commands.RemovePicture;

public sealed record RemoveProducerPictureCommand : ICommand<Success>, ITransactional;