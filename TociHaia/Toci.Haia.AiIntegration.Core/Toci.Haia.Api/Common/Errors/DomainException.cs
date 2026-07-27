using System.Net;

namespace Toci.Haia.Api.Common.Errors;

public sealed class DomainException : Exception
{
    public DomainException(HttpStatusCode statusCode, string code, string message)
        : base(message)
    {
        StatusCode = statusCode;
        Code = code;
    }

    public HttpStatusCode StatusCode { get; }
    public string Code { get; }
}
