using Eventify.Application.Common.Abstractions.Storage;

namespace Eventify.Presentation.Common.Requests;

internal sealed class FileProxy(IFormFile file) : IFile
{
    public string Name => file.Name;

    public string Extension => Path.GetExtension(file.FileName);

    public string ContentType => file.ContentType;

    public Stream Stream => file.OpenReadStream();
}