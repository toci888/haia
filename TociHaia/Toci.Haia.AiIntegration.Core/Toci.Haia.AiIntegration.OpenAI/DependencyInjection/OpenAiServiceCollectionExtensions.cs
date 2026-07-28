namespace Toci.Haia.AiIntegration.OpenAI.DependencyInjection;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Toci.Haia.AiIntegration.Core.Abstractions;
using Toci.Haia.AiIntegration.Core.Operations.EvaluateStudioMemeImage;
using Toci.Haia.AiIntegration.OpenAI.Configuration;
using Toci.Haia.AiIntegration.OpenAI.Operations;

public static class OpenAiServiceCollectionExtensions
{
    public static IServiceCollection AddHaiaOpenAiIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<OpenAiIntegrationOptions>()
            .Bind(configuration.GetSection(OpenAiIntegrationOptions.SectionName));

        services.AddSingleton<IValidateOptions<OpenAiIntegrationOptions>, OpenAiOptionsValidator>();

        services.AddSingleton(sp => sp.GetRequiredService<IOptions<OpenAiIntegrationOptions>>().Value);
        services.AddSingleton(new EvaluateStudioMemeImageValidationSettings());
        services.AddSingleton<EvaluateStudioMemeImageRequestValidator>();
        services.AddSingleton<EvaluateStudioMemeImageResponseValidator>();

        services.AddScoped<IAiOperation<EvaluateStudioMemeImageRequest, EvaluateStudioMemeImageResponse>, EvaluateStudioMemeImageOpenAiOperation>();

        return services;
    }
}
