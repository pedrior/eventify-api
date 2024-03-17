namespace Eventify.Infrastructure.Common.Storage;

internal sealed class StorageOptions
{
    public const string SectionName = "Storage";

    public required string Bucket { get; init; }
}