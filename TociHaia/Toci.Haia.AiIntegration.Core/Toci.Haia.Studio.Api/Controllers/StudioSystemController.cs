using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Toci.Haia.Studio.Api.Contracts;

namespace Toci.Haia.Studio.Api.Controllers;

[ApiController]
[Route("api/v1/system")]
[EnableRateLimiting("studio-default")]
public sealed class StudioSystemController : ControllerBase
{
    [HttpGet("info", Name = "GetStudioSystemInfo")]
    [ProducesResponseType(typeof(SystemInfoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Get Studio API system information")]
    [EndpointDescription("Returns minimal non-sensitive status payload to validate Studio API host, routing and OpenAPI discovery.")]
    public ActionResult<SystemInfoResponse> GetInfo(CancellationToken cancellationToken)
    {
        var apiVersion = typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0";
        var response = new SystemInfoResponse(
            ServiceName: "HAIA Studio API",
            ApiVersion: apiVersion,
            Status: "ok");
        return Ok(response);
    }
}
