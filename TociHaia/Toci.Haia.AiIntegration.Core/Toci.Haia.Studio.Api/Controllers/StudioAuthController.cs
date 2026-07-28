using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Toci.Haia.Studio.Api.Common.Errors;
using Toci.Haia.Studio.Api.Contracts;
using Toci.Haia.Studio.Api.Features.Authentication;

namespace Toci.Haia.Studio.Api.Controllers;

[ApiController]
[Route("api/v1/studio/auth")]
[EnableRateLimiting("studio-default")]
public sealed class StudioAuthController(
    IStudioAuthenticationService authenticationService,
    IAntiforgery antiforgery,
    TimeProvider timeProvider,
    ILogger<StudioAuthController> logger) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("csrf", Name = "GetStudioCsrfToken")]
    [ProducesResponseType(typeof(StudioCsrfTokenResponse), StatusCodes.Status200OK)]
    public ActionResult<StudioCsrfTokenResponse> GetCsrfToken(CancellationToken cancellationToken)
    {
        var tokens = antiforgery.GetAndStoreTokens(HttpContext);
        if (string.IsNullOrWhiteSpace(tokens.RequestToken))
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.InternalServerError,
                ErrorCodes.UnexpectedError,
                "Unexpected error",
                "Unable to issue csrf token.");
        }

        return Ok(new StudioCsrfTokenResponse(tokens.RequestToken));
    }

    [AllowAnonymous]
    [EnableRateLimiting("studio-login")]
    [HttpPost("login", Name = "StudioLogin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] StudioLoginRequest request, CancellationToken cancellationToken)
    {
        try
        {
            await antiforgery.ValidateRequestAsync(HttpContext);
        }
        catch (AntiforgeryValidationException)
        {
            return BuildProblem(
                StatusCodes.Status400BadRequest,
                ErrorCodes.CsrfValidationFailed,
                "CSRF validation failed",
                "Request validation failed.");
        }

        var loginResult = await authenticationService.AuthenticateAsync(request.Email, request.Password, cancellationToken);
        if (!loginResult.Succeeded || loginResult.Session is null)
        {
            logger.LogWarning("Studio login denied with generic response. Reason={Reason}", loginResult.FailureReason);
            return BuildProblem(
                StatusCodes.Status401Unauthorized,
                ErrorCodes.InvalidCredentials,
                "Invalid credentials",
                "Nie udało się zalogować. Sprawdź dane albo skontaktuj się z administratorem.");
        }

        var session = loginResult.Session;
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, session.AccountId.ToString("D")),
            new(ClaimTypes.Name, session.DisplayName),
            new(ClaimTypes.Email, session.Email),
            new("account_status", session.AccountStatus),
            new("session_started_at", timeProvider.GetUtcNow().UtcDateTime.ToString("O")),
        };

        foreach (var role in session.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        foreach (var scope in session.Scopes)
        {
            claims.Add(new Claim("scope", scope));
        }

        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        return NoContent();
    }

    [Authorize(Policy = "studio-access")]
    [HttpGet("status", Name = "GetStudioAuthStatus")]
    [ProducesResponseType(typeof(StudioAuthStatusResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [EndpointSummary("Get Studio authentication status")]
    [EndpointDescription("Returns normalized identity information for authenticated Studio principals.")]
    public ActionResult<StudioAuthStatusResponse> GetStatus(CancellationToken cancellationToken)
    {
        var accountIdRaw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        _ = Guid.TryParse(accountIdRaw, out var accountId);

        var userName = User.Identity?.Name
            ?? User.FindFirstValue(ClaimTypes.Name)
            ?? User.FindFirstValue("sub")
            ?? "unknown";
        var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;

        var roles = User.FindAll(ClaimTypes.Role).Select(claim => claim.Value).Distinct(StringComparer.Ordinal).ToArray();
        var scopes = User.FindAll("scope")
            .SelectMany(claim => claim.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        var response = new StudioAuthStatusResponse(
            AccountId: accountId,
            User: userName,
            Email: email,
            Roles: roles,
            Scopes: scopes,
            Status: "authorized");

        return Ok(response);
    }

    [Authorize(Policy = "studio-access")]
    [HttpPost("logout", Name = "StudioLogout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Logout(CancellationToken cancellationToken)
    {
        try
        {
            await antiforgery.ValidateRequestAsync(HttpContext);
        }
        catch (AntiforgeryValidationException)
        {
            return BuildProblem(
                StatusCodes.Status400BadRequest,
                ErrorCodes.CsrfValidationFailed,
                "CSRF validation failed",
                "Request validation failed.");
        }

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    private ObjectResult BuildProblem(int statusCode, string code, string title, string detail)
    {
        var problem = new ProblemDetails
        {
            Type = "about:blank",
            Title = title,
            Detail = detail,
            Status = statusCode,
            Instance = HttpContext.Request.Path,
        };
        problem.Extensions["code"] = code;

        return StatusCode(statusCode, problem);
    }
}
