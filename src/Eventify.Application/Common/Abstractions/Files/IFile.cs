namespace Eventify.Application.Common.Abstractions.Files;

public interface IFile
{
    string Name { get; }
    
    string Extension { get; }
    
    string ContentType { get; }
    
    Stream Stream { get; }
}