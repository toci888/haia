namespace Toci.Haia.Studio.Api.Contracts;

public sealed record StudioAuthStatusResponse(
    Guid AccountId,
    string User,
    string Email,
    IReadOnlyCollection<string> Roles,
    IReadOnlyCollection<string> Scopes,
    string Status);
