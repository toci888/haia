using Microsoft.AspNetCore.Mvc;
using Toci.Haia.Api.Features.LegalDocuments;
using Toci.Haia.Api.Features.LegalDocuments.Contracts;

namespace Toci.Haia.Api.Controllers;

[ApiController]
[Route("api/v1/legal-documents")]
public sealed class LegalDocumentsController(ILegalDocumentsService service) : ControllerBase
{
    [HttpGet("current", Name = "GetCurrentLegalDocuments")]
    [ProducesResponseType(typeof(CurrentLegalDocumentsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    [EndpointSummary("Get current legal documents")]
    [EndpointDescription("Returns currently active legal document versions for the selected or default language.")]
    public async Task<ActionResult<CurrentLegalDocumentsResponse>> GetCurrent([FromQuery] string? language, CancellationToken cancellationToken)
    {
        var response = await service.GetCurrentAsync(language, cancellationToken);
        return Ok(response);
    }
}
