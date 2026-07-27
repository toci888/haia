namespace Toci.Haia.Studio.Api.Contracts;

public sealed record SystemInfoResponse(
    string ServiceName,
    string ApiVersion,
    string Status);
