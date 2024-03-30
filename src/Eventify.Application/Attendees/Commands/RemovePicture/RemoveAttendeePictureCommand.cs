using Eventify.Application.Common.Abstractions.Data;

namespace Eventify.Application.Attendees.Commands.RemovePicture;

public sealed record RemoveAttendeePictureCommand : ICommand<Success>, ITransactional;