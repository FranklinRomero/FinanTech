using FinanTech_Solutions.Application.DTOs;
using FinanTech_Solutions.Application.Interfaces;
using FinanTech_Solutions.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FinanTech_Solutions.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReportsController(IReportOrchestrator orchestrator) : ControllerBase
{
    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] ReportRequest request)
    {
        var result = await orchestrator.GenerateAsync(request);

        if (!result.Success)
            return BadRequest(new { result.Message });

        if (request.Channel == DeliveryChannel.Api && result.InlineContent is not null)
            return File(result.InlineContent, result.ContentType!, $"report_{result.ReportId}");

        return Ok(result);
    }
}
