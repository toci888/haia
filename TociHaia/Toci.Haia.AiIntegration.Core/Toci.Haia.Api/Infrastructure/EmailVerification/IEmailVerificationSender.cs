namespace Toci.Haia.Api.Infrastructure.EmailVerification;

public interface IEmailVerificationSender
{
    Task SendAsync(EmailVerificationMessage message, CancellationToken cancellationToken);
}
