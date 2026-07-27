namespace Toci.Haia.Api.Features.Registration.Email.Contracts;

public sealed record ResendEmailRequest(string Email, string? Language);
