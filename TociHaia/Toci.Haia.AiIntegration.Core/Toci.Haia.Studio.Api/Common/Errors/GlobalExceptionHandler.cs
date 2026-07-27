using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace Toci.Haia.Studio.Api.Common.Errors;

public sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, code, title, detail) = Map(exception);
        logger.LogError(exception, "Studio request failed with code {Code}", code);

        httpContext.Response.StatusCode = (int)status;
        var problem = new ProblemDetails
        {
            Type = "about:blank",
            Title = title,
            Detail = detail,
            Status = (int)status,
            Instance = httpContext.Request.Path,
        };
        problem.Extensions["code"] = code;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = problem,
            Exception = exception,
        });
    }

    private static (HttpStatusCode status, string code, string title, string detail) Map(Exception exception)
    {
        if (exception is NpgsqlException)
        {
            return (HttpStatusCode.ServiceUnavailable, ErrorCodes.DatabaseUnavailable, "Database unavailable", "Database operation failed.");
        }

        return (HttpStatusCode.InternalServerError, ErrorCodes.UnexpectedError, "Unexpected error", "Unexpected server error.");
    }
}
