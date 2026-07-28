namespace Toci.Haia.Studio.Api.Contracts;

public sealed record StudioLoginRequest(
    string Email,
    string Password);
