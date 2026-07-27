using System.Text.RegularExpressions;
using Microsoft.Extensions.Primitives;

namespace Toci.Haia.Studio.Api.Common.Correlation;

public sealed class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    private static readonly Regex AllowedPattern = new("^[a-zA-Z0-9._\\-]+$", RegexOptions.Compiled);

    public async Task Invoke(HttpContext context)
    {
        var correlationId = ResolveCorrelationId(context.Request.Headers);
        context.Items[CorrelationConstants.ItemKey] = correlationId;
        context.Response.Headers[CorrelationConstants.HeaderName] = correlationId;

        var startedAt = DateTime.UtcNow;
        using (logger.BeginScope(new Dictionary<string, object?> { [CorrelationConstants.ItemKey] = correlationId }))
        {
            await next(context);
            var elapsedMs = (DateTime.UtcNow - startedAt).TotalMilliseconds;
            logger.LogInformation("Studio request {Method} {Path} responded {StatusCode} in {ElapsedMs}ms (corr={CorrelationId})",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                Math.Round(elapsedMs, 2),
                correlationId);
        }
    }

    private static string ResolveCorrelationId(IHeaderDictionary headers)
    {
        if (headers.TryGetValue(CorrelationConstants.HeaderName, out StringValues value)
            && !StringValues.IsNullOrEmpty(value))
        {
            var candidate = value.ToString().Trim();
            if (!string.IsNullOrWhiteSpace(candidate)
                && candidate.Length <= CorrelationConstants.MaxLength
                && AllowedPattern.IsMatch(candidate))
            {
                return candidate;
            }
        }

        return Guid.NewGuid().ToString("N");
    }
}
