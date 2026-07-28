using System.Security.Claims;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Toci.Haia.Studio.Api.Common.Correlation;
using Toci.Haia.Studio.Api.Common.Errors;
using Toci.Haia.Studio.Api.Features.MemeIntakes;

namespace Toci.Haia.Studio.Api.Controllers;

[ApiController]
[Authorize(Policy = "studio-access")]
[Route("api/v1/studio/meme-intakes")]
[EnableRateLimiting("studio-default")]
public sealed class MemeIntakesController(
    IMemeIntakeService service,
    IAntiforgery antiforgery) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(CreateMemeIntakeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CreateMemeIntakeResponse>> Create([FromBody] CreateMemeIntakeRequest request, CancellationToken cancellationToken)
    {
        await ValidateCsrfAsync();

        var accountRaw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(accountRaw, out var accountId))
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.Unauthorized,
                ErrorCodes.InvalidCredentials,
                "Invalid credentials",
                "Studio account identifier is missing.");
        }

        var response = await service.CreateAsync(accountId, request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{intakeId:guid}/finalize-upload")]
    [ProducesResponseType(typeof(FinalizeMemeIntakeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FinalizeMemeIntakeResponse>> FinalizeUpload(Guid intakeId, CancellationToken cancellationToken)
    {
        await ValidateCsrfAsync();
        var response = await service.FinalizeAsync(intakeId, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{intakeId:guid}/evaluate")]
    [ProducesResponseType(typeof(EvaluateMemeIntakeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<EvaluateMemeIntakeResponse>> Evaluate(Guid intakeId, CancellationToken cancellationToken)
    {
        await ValidateCsrfAsync();
        var correlationId = HttpContext.Items[CorrelationConstants.ItemKey] as string ?? Guid.NewGuid().ToString("N");
        var response = await service.EvaluateAsync(intakeId, correlationId, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{intakeId:guid}/editorial-review/start")]
    [ProducesResponseType(typeof(StartEditorialReviewResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StartEditorialReviewResponse>> StartEditorialReview(Guid intakeId, CancellationToken cancellationToken)
    {
        await ValidateCsrfAsync();
        var accountId = ReadAccountId();
        var response = await service.StartEditorialReviewAsync(intakeId, accountId, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{intakeId:guid}/editorial-review/approve")]
    [ProducesResponseType(typeof(MemeEditorialDecisionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MemeEditorialDecisionResponse>> Approve(Guid intakeId, [FromBody] MemeEditorialDecisionRequest request, CancellationToken cancellationToken)
    {
        await ValidateCsrfAsync();
        var accountId = ReadAccountId();
        var response = await service.ApproveAsync(intakeId, accountId, request, cancellationToken);
        return Ok(response);
    }

    [HttpPost("{intakeId:guid}/editorial-review/reject")]
    [ProducesResponseType(typeof(MemeEditorialDecisionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MemeEditorialDecisionResponse>> Reject(Guid intakeId, [FromBody] MemeEditorialDecisionRequest request, CancellationToken cancellationToken)
    {
        await ValidateCsrfAsync();
        var accountId = ReadAccountId();
        var response = await service.RejectAsync(intakeId, accountId, request, cancellationToken);
        return Ok(response);
    }

    [HttpGet("{intakeId:guid}")]
    [ProducesResponseType(typeof(GetMemeIntakeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GetMemeIntakeResponse>> Get(Guid intakeId, CancellationToken cancellationToken)
    {
        var response = await service.GetAsync(intakeId, cancellationToken);
        return Ok(response);
    }

    private async Task ValidateCsrfAsync()
    {
        try
        {
            await antiforgery.ValidateRequestAsync(HttpContext);
        }
        catch (AntiforgeryValidationException)
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.BadRequest,
                ErrorCodes.CsrfValidationFailed,
                "CSRF validation failed",
                "Request validation failed.");
        }
    }

    private Guid ReadAccountId()
    {
        var accountRaw = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(accountRaw, out var accountId))
        {
            throw new StudioProblemDetailsException(
                System.Net.HttpStatusCode.Unauthorized,
                ErrorCodes.InvalidCredentials,
                "Invalid credentials",
                "Studio account identifier is missing.");
        }

        return accountId;
    }
}
