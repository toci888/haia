using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Toci.Haia.Api.Common.Correlation;
using Toci.Haia.Api.Common.Errors;
using Toci.Haia.Api.Features.Registration.Email;
using Toci.Haia.Api.Features.Registration.Email.Contracts;
using Toci.Haia.Api.Features.Registration.Nickname;
using Toci.Haia.Api.Features.Registration.Nickname.Contracts;

namespace Toci.Haia.Api.Controllers;

[ApiController]
[Route("api/v1/registration")]
public sealed class RegistrationController(
    INicknameAvailabilityService nicknameAvailabilityService,
    IEmailRegistrationService emailRegistrationService) : ControllerBase
{
    [HttpGet("nickname-availability", Name = "CheckNicknameAvailability")]
    [EnableRateLimiting("nickname")]
    [ProducesResponseType(typeof(NicknameAvailabilityResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Check nickname availability")]
    [EndpointDescription("Checks if the requested nickname can be used for a new account.")]
    public async Task<ActionResult<NicknameAvailabilityResponse>> CheckNicknameAvailability([FromQuery] string nickname, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(nickname))
        {
            throw new DomainException(HttpStatusCode.UnprocessableEntity, ErrorCodes.ValidationFailed, "Nickname is required.");
        }

        var response = await nicknameAvailabilityService.CheckAsync(nickname, cancellationToken);
        return Ok(response);
    }

    [HttpPost("email", Name = "RegisterByEmail")]
    [EnableRateLimiting("registration")]
    [ProducesResponseType(typeof(AcceptedResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status503ServiceUnavailable)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Register account by email and password")]
    [EndpointDescription("Starts email registration flow and sends verification token when possible.")]
    public async Task<ActionResult<AcceptedResponse>> RegisterByEmail([FromBody] RegisterEmailRequest request, CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.Items[CorrelationConstants.ItemKey]?.ToString() ?? Guid.NewGuid().ToString("N");
        var result = await emailRegistrationService.RegisterAsync(request, correlationId, cancellationToken);

        return result switch
        {
            RegistrationResult.Accepted => Accepted(value: new AcceptedResponse()),
            RegistrationResult.NicknameUnavailable => Problem(statusCode: 409, title: "Nickname unavailable", extensions: new Dictionary<string, object?> { ["code"] = ErrorCodes.NicknameUnavailable }),
            RegistrationResult.LegalDocumentsChanged => Problem(statusCode: 409, title: "Legal documents changed", extensions: new Dictionary<string, object?> { ["code"] = ErrorCodes.LegalDocumentsChanged }),
            RegistrationResult.LegalDecisionsIncomplete => Problem(statusCode: 422, title: "Legal decisions incomplete", extensions: new Dictionary<string, object?> { ["code"] = ErrorCodes.LegalDecisionsIncomplete }),
            RegistrationResult.VerificationDeliveryUnavailable => Problem(statusCode: 503, title: "Verification delivery unavailable", extensions: new Dictionary<string, object?> { ["code"] = ErrorCodes.VerificationDeliveryUnavailable }),
            _ => Problem(statusCode: 500, title: "Unexpected error", extensions: new Dictionary<string, object?> { ["code"] = ErrorCodes.UnexpectedError })
        };
    }

    [HttpPost("email/verify", Name = "VerifyEmailRegistration")]
    [EnableRateLimiting("verification")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Verify email token")]
    [EndpointDescription("Completes email verification for pending registration token.")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request, CancellationToken cancellationToken)
    {
        await emailRegistrationService.VerifyAsync(request, cancellationToken);
        return NoContent();
    }

    [HttpPost("email/resend", Name = "ResendEmailVerification")]
    [EnableRateLimiting("resend")]
    [ProducesResponseType(typeof(AcceptedResponse), StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Resend email verification token")]
    [EndpointDescription("Requests sending a new verification token for pending email registration.")]
    public async Task<ActionResult<AcceptedResponse>> ResendEmail([FromBody] ResendEmailRequest request, CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.Items[CorrelationConstants.ItemKey]?.ToString() ?? Guid.NewGuid().ToString("N");
        await emailRegistrationService.ResendAsync(request, correlationId, cancellationToken);
        return Accepted(value: new AcceptedResponse());
    }
}
