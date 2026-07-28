namespace Toci.Haia.Studio.Api.Features.MemeIntakes;

public sealed class MemeIntakeOptions
{
    public const string SectionName = "MemeIntake";

    public long MaxFileSizeBytes { get; init; } = 10 * 1024 * 1024;

    public int UploadUrlTtlMinutes { get; init; } = 10;

    public string[] AllowedContentTypes { get; init; } = ["image/jpeg", "image/png", "image/webp"];
}

public sealed class R2StorageOptions
{
    public const string SectionName = "R2Storage";

    public string? Endpoint { get; init; }

    public string? BucketName { get; init; }

    public string? AccessKeyId { get; init; }

    public string? SecretAccessKey { get; init; }

    public string? Region { get; init; }

    public bool IsConfigured =>
        !string.IsNullOrWhiteSpace(Endpoint)
        && !string.IsNullOrWhiteSpace(BucketName)
        && !string.IsNullOrWhiteSpace(AccessKeyId)
        && !string.IsNullOrWhiteSpace(SecretAccessKey);
}
