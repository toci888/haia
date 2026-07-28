namespace Toci.Haia.Studio.Api.Features.MemeIntakes;

public interface IMediaObjectStorage
{
    Task<MediaUploadIntent> CreateUploadIntentAsync(
        string objectKey,
        string contentType,
        DateTimeOffset expiresAtUtc,
        CancellationToken cancellationToken);

    Task<MediaObjectMetadata> GetObjectMetadataAsync(string objectKey, CancellationToken cancellationToken);

    Task<byte[]> ReadObjectPrefixAsync(string objectKey, int maxBytes, CancellationToken cancellationToken);

    Task<byte[]> ReadObjectBytesAsync(string objectKey, long maxBytes, CancellationToken cancellationToken);
}

public sealed record MediaUploadIntent(
    string UploadUrl,
    IReadOnlyDictionary<string, string> UploadHeaders,
    DateTimeOffset ExpiresAtUtc);

public sealed record MediaObjectMetadata(
    bool Exists,
    string? ContentType,
    long SizeBytes,
    string? ETag);
