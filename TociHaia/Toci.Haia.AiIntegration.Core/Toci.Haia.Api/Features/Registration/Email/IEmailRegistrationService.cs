using Toci.Haia.Api.Features.Registration.Email.Contracts;

namespace Toci.Haia.Api.Features.Registration.Email;

public interface IEmailRegistrationService
{
    Task<RegistrationResult> RegisterAsync(RegisterEmailRequest request, string correlationId, CancellationToken cancellationToken);
    Task VerifyAsync(VerifyEmailRequest request, CancellationToken cancellationToken);
    Task ResendAsync(ResendEmailRequest request, string correlationId, CancellationToken cancellationToken);
}
