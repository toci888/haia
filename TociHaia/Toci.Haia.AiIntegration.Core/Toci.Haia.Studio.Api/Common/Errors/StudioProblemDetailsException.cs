using System.Net;

namespace Toci.Haia.Studio.Api.Common.Errors;

public sealed class StudioProblemDetailsException(
    HttpStatusCode statusCode,
    string code,
    string title,
    string detail) : Exception(detail)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
    public string Code { get; } = code;
    public string Title { get; } = title;
    public string Detail { get; } = detail;
}
