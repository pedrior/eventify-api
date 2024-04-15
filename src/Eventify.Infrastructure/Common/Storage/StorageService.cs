using System.Net;
using Amazon.S3;
using Amazon.S3.Model;
using Eventify.Application.Common.Abstractions.Storage;
using Microsoft.Extensions.Options;

namespace Eventify.Infrastructure.Common.Storage;

internal sealed class StorageService(IAmazonS3 s3, IOptions<StorageOptions> options) : IStorageService
{
    private readonly StorageOptions options = options.Value;

    private readonly string regionName = s3.Config.RegionEndpoint.SystemName;

    public async Task<Uri?> UploadAsync(Uri key, IFile file, CancellationToken cancellationToken = default)
    {
        var request = new PutObjectRequest
        {
            Key = key.ToString(),
            ContentType = file.ContentType,
            InputStream = file.Stream,
            BucketName = options.Bucket,
            Metadata =
            {
                ["x-amz-meta-name"] = file.Name,
                ["x-amx-meta-extension"] = file.Extension
            }
        };

        var response = await s3.PutObjectAsync(request, cancellationToken);
        return response.HttpStatusCode is HttpStatusCode.OK ? GetFileUrl(key) : null;
    }

    public async Task<bool> DeleteAsync(Uri key, CancellationToken cancellationToken = default)
    {
        var request = new DeleteObjectRequest
        {
            Key = key.ToString(),
            BucketName = options.Bucket
        };

        var response = await s3.DeleteObjectAsync(request, cancellationToken);
        return response.HttpStatusCode is HttpStatusCode.NoContent;
    }

    private Uri GetFileUrl(Uri key) => new($"https://{options.Bucket}.s3.{regionName}.amazonaws.com/{key}");
}