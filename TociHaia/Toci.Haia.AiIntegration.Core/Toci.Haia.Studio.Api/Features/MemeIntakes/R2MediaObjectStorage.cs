namespace Toci.Haia.Studio.Api.Features.MemeIntakes;

using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Toci.Haia.Studio.Api.Common.Errors;

public sealed class R2MediaObjectStorage(
    R2StorageOptions options,
    ILogger<R2MediaObjectStorage> logger) : IMediaObjectStorage
{
    private readonly R2StorageOptions _options = options;
    private readonly ILogger<R2MediaObjectStorage> _logger = logger;

    public Task<MediaUploadIntent> CreateUploadIntentAsync(
        string objectKey,
        string contentType,
        DateTimeOffset expiresAtUtc,
        CancellationToken cancellationToken)
    {
        var client = CreateClient();
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _options.BucketName,
            Key = objectKey,
            Verb = HttpVerb.PUT,
            Expires = expiresAtUtc.UtcDateTime,
            ContentType = contentType,
            Protocol = Protocol.HTTPS,
        };

        var url = client.GetPreSignedURL(request);
        var headers = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Content-Type"] = contentType,
        };

        return Task.FromResult(new MediaUploadIntent(url, headers, expiresAtUtc));
    }

    public async Task<MediaObjectMetadata> GetObjectMetadataAsync(string objectKey, CancellationToken cancellationToken)
    {
        var client = CreateClient();
        try
        {
            var response = await client.GetObjectMetadataAsync(new GetObjectMetadataRequest
            {
                BucketName = _options.BucketName,
                Key = objectKey,
            }, cancellationToken);

            return new MediaObjectMetadata(
                Exists: true,
                ContentType: response.Headers.ContentType,
                SizeBytes: response.Headers.ContentLength,
                ETag: response.ETag);
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return new MediaObjectMetadata(false, null, 0, null);
        }
    }

    public async Task<byte[]> ReadObjectPrefixAsync(string objectKey, int maxBytes, CancellationToken cancellationToken)
    {
        var client = CreateClient();
        var response = await client.GetObjectAsync(new GetObjectRequest
        {
            BucketName = _options.BucketName,
            Key = objectKey,
            ByteRange = new ByteRange(0, Math.Max(0, maxBytes - 1)),
        }, cancellationToken);

        await using var stream = response.ResponseStream;
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms, cancellationToken);
        return ms.ToArray();
    }

    public async Task<byte[]> ReadObjectBytesAsync(string objectKey, long maxBytes, CancellationToken cancellationToken)
    {
        var metadata = await GetObjectMetadataAsync(objectKey, cancellationToken);
        if (!metadata.Exists)
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.BadRequest,
                ErrorCodes.UploadObjectMissing,
                "Upload object missing",
                "Uploaded object was not found in storage.");
        }

        if (metadata.SizeBytes > maxBytes)
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.BadRequest,
                ErrorCodes.FileTooLarge,
                "File too large",
                "Uploaded file exceeds configured size limit.");
        }

        var client = CreateClient();
        var response = await client.GetObjectAsync(new GetObjectRequest
        {
            BucketName = _options.BucketName,
            Key = objectKey,
        }, cancellationToken);

        await using var stream = response.ResponseStream;
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms, cancellationToken);

        _logger.LogInformation("Read object {ObjectKey} from private R2.", objectKey);
        return ms.ToArray();
    }

    private IAmazonS3 CreateClient()
    {
        if (!_options.IsConfigured)
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.ServiceUnavailable,
                ErrorCodes.StorageNotConfigured,
                "Storage not configured",
                "Private object storage is not configured in this environment.");
        }

        var credentials = new BasicAWSCredentials(_options.AccessKeyId, _options.SecretAccessKey);
        var config = new AmazonS3Config
        {
            ServiceURL = _options.Endpoint,
            ForcePathStyle = true,
            AuthenticationRegion = _options.Region ?? RegionEndpoint.USEast1.SystemName,
        };

        return new AmazonS3Client(credentials, config);
    }
}
