namespace Toci.Haia.AiIntegration.OpenAI.Prompting;

internal sealed record AiPromptTemplate(
    string TemplateId,
    string Version,
    string SystemPrompt);