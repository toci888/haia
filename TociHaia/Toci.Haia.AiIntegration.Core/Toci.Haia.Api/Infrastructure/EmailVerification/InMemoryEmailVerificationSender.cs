using System.Collections.Concurrent;

namespace Toci.Haia.Api.Infrastructure.EmailVerification;

public sealed class InMemoryEmailVerificationSender : IEmailVerificationSender
{
    private readonly ConcurrentQueue<EmailVerificationMessage> _messages = new();

    public Task SendAsync(EmailVerificationMessage message, CancellationToken cancellationToken)
    {
        _messages.Enqueue(message);
        return Task.CompletedTask;
    }

    public IReadOnlyCollection<EmailVerificationMessage> Snapshot() => _messages.ToArray();
}
