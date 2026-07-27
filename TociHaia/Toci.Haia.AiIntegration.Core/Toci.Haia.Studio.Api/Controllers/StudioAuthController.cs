using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Toci.Haia.Studio.Api.Contracts;

namespace Toci.Haia.Studio.Api.Controllers;

[ApiController]
[Route("api/v1/studio/auth")]
[EnableRateLimiting("studio-default")]
public sealed class StudioAuthController : ControllerBase
{
    [Authorize(Policy = "studio-access")]
    [HttpGet("status", Name = "GetStudioAuthStatus")]
    [ProducesResponseType(typeof(StudioAuthStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [EndpointSummary("Get Studio authentication status")]
    [EndpointDescription("Returns normalized identity information for authenticated Studio principals.")]
    public ActionResult<StudioAuthStatusResponse> GetStatus(CancellationToken cancellationToken)
    {
        var userName = User.Identity?.Name
            ?? User.FindFirstValue(ClaimTypes.Name)
            ?? User.FindFirstValue("sub")
            ?? "unknown";

        var roles = User.FindAll(ClaimTypes.Role).Select(claim => claim.Value).Distinct(StringComparer.Ordinal).ToArray();
        var scopes = User.FindAll("scope")
            .SelectMany(claim => claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        var response = new StudioAuthStatusResponse(
            User: userName,
            Roles: roles,
            Scopes: scopes,
            Status: "authorized");

        return Ok(response);
    }
}
