namespace Toci.Haia.AiIntegration.OpenAI.Configuration;

using Microsoft.Extensions.Options;

internal sealed class OpenAiOptionsValidator : IValidateOptions<OpenAiIntegrationOptions>
{
    public ValidateOptionsResult Validate(string? name, OpenAiIntegrationOptions options)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(options.Model))
        {
            errors.Add("OpenAI model is missing.");
        }

        if (options.MaxImageBytes <= 0)
        {
            errors.Add("MaxImageBytes must be greater than zero.");
        }

        if (options.MaxReactionCount <= 0)
        {
            errors.Add("MaxReactionCount must be greater than zero.");
        }

        if (options.DefaultReactionCount <= 0 || options.DefaultReactionCount > options.MaxReactionCount)
        {
            errors.Add("DefaultReactionCount must be in range 1..MaxReactionCount.");
        }

        if (options.MaxRetryAttempts < 1)
        {
            errors.Add("MaxRetryAttempts must be at least 1.");
        }

        if (options.MaxRepairAttempts < 0)
        {
            errors.Add("MaxRepairAttempts cannot be negative.");
        }

        if (options.Timeout <= TimeSpan.Zero)
        {
            errors.Add("Timeout must be greater than zero.");
        }

        return errors.Count > 0 ? ValidateOptionsResult.Fail(errors) : ValidateOptionsResult.Success;
    }
}