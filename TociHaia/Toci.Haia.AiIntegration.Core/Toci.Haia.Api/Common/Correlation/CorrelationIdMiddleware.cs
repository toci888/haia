using Microsoft.Extensions.Primitives;

namespace Toci.Haia.Api.Common.Correlation;

public sealed class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        var correlationId = ResolveCorrelationId(context.Request.Headers);
        context.Items[CorrelationConstants.ItemKey] = correlationId;
        context.Response.Headers[CorrelationConstants.HeaderName] = correlationId;

        using (logger.BeginScope(new Dictionary<string, object?> { [CorrelationConstants.ItemKey] = correlationId }))
        {
            await next(context);
        }
    }

    private static string ResolveCorrelationId(IHeaderDictionary headers)
    {
        if (headers.TryGetValue(CorrelationConstants.HeaderName, out StringValues value)
            && !StringValues.IsNullOrEmpty(value)
            && !string.IsNullOrWhiteSpace(value.ToString()))
        {
            return value.ToString();
        }

        return Guid.NewGuid().ToString("N");
    }
}
