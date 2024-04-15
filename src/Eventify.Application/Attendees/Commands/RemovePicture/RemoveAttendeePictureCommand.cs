using Eventify.Application.Common.Abstractions.Persistence;

namespace Eventify.Application.Attendees.Commands.RemovePicture;

public sealed record RemoveAttendeePictureCommand : ICommand<Success>, ITransactional;