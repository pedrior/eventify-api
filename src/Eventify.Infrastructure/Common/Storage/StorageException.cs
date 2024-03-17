namespace Eventify.Infrastructure.Common.Storage;

internal sealed class StorageException(string message, Exception? innerException = null) 
    : Exception(message, innerException);