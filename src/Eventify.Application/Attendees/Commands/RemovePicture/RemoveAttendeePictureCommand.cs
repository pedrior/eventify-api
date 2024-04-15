using Eventify.Application.Common.Abstractions.Persistence;
using Eventify.Application.Common.Abstractions.Requests;

namespace Eventify.Application.Attendees.Commands.RemovePicture;

public sealed record RemoveAttendeePictureCommand : ICommand<Success>, ITransactional;