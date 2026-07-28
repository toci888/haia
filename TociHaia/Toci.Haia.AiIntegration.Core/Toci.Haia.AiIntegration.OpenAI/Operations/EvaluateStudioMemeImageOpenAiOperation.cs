namespace Toci.Haia.AiIntegration.OpenAI.Operations;

using Toci.Haia.AiIntegration.Core.Abstractions;
using Toci.Haia.AiIntegration.Core.Models;
using Toci.Haia.AiIntegration.Core.Operations.EvaluateStudioMemeImage;
using Toci.Haia.AiIntegration.OpenAI.Configuration;

public sealed class EvaluateStudioMemeImageOpenAiOperation(
    OpenAiIntegrationOptions options,
    EvaluateStudioMemeImageRequestValidator requestValidator,
    EvaluateStudioMemeImageResponseValidator responseValidator)
    : IAiOperation<EvaluateStudioMemeImageRequest, EvaluateStudioMemeImageResponse>
{
    public Task<AiOperationResult<EvaluateStudioMemeImageResponse>> ExecuteAsync(
        EvaluateStudioMemeImageRequest request,
        CancellationToken cancellationToken = default)
    {
        var startedAt = DateTimeOffset.UtcNow;

        var requestIssues = requestValidator.Validate(request, cancellationToken);
        if (requestIssues.Count > 0)
        {
            return Task.FromResult(
                AiOperationResult<EvaluateStudioMemeImageResponse>.Failure(
                    new AiOperationError(
                        AiErrorCode.ValidationFailed,
                        string.Join("; ", requestIssues.Select(x => $"{x.Path}: {x.Message}"))),
                    BuildMetadata(startedAt, DateTimeOffset.UtcNow, AiExecutionStatus.Failed)));
        }

        if (string.IsNullOrWhiteSpace(options.ApiKey) || string.IsNullOrWhiteSpace(options.Model))
        {
            return Task.FromResult(
                AiOperationResult<EvaluateStudioMemeImageResponse>.Failure(
                    new AiOperationError(
                        AiErrorCode.ConfigurationMissing,
                        "OpenAI configuration is missing. Set API key and model in runtime configuration."),
                    BuildMetadata(startedAt, DateTimeOffset.UtcNow, AiExecutionStatus.Failed)));
        }

        return Task.FromResult(
            AiOperationResult<EvaluateStudioMemeImageResponse>.Failure(
                new AiOperationError(
                    AiErrorCode.ProviderUnavailable,
                    "Runtime OpenAI call is not verified in this environment."),
                BuildMetadata(startedAt, DateTimeOffset.UtcNow, AiExecutionStatus.Failed)));
    }

    private static AiOperationMetadata BuildMetadata(DateTimeOffset startedAt, DateTimeOffset finishedAt, AiExecutionStatus status)
    {
        return new AiOperationMetadata
        {
            OperationName = "EvaluateStudioMemeImage",
            CorrelationId = Guid.NewGuid().ToString("N"),
            Provider = "openai",
            Model = "not-configured",
            PromptTemplateId = "studio-meme-evaluation",
            PromptTemplateVersion = "v0.1",
            StartedAtUtc = startedAt,
            FinishedAtUtc = finishedAt,
            Duration = finishedAt - startedAt,
            Status = status,
        };
    }
}
