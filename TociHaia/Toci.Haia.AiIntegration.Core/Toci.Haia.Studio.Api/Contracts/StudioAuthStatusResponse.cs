namespace Toci.Haia.Studio.Api.Contracts;

public sealed record StudioAuthStatusResponse(
    string User,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Scopes,
    string Status);
