using Eventify.Application.Common.Abstractions.Files;

namespace Eventify.Application.Common.Abstractions.Storage;

public interface IStorageService
{
    Task<Uri?> UploadAsync(Uri key, IFile file, CancellationToken cancellationToken = default);
    
    Task<bool> DeleteAsync(Uri key, CancellationToken cancellationToken = default);
}