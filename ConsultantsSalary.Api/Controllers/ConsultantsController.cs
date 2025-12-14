using ConsultantsSalary.Application.Features.Consultants.Commands;
using ConsultantsSalary.Application.Features.Consultants.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsultantsSalary.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ConsultantsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ConsultantsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<ConsultantDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllConsultantsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<ConsultantDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetConsultantByIdQuery(id), ct);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<ConsultantDto>> Create([FromBody] CreateConsultantCommand request, CancellationToken ct)
    {
        try
        {
            var result = await _mediator.Send(request, ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Email already exists"))
        {
            return Conflict(new { error = ex.Message });
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Role not found"))
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<ConsultantDto>> Update([FromRoute] Guid id, [FromBody] UpdateConsultantCommand request, CancellationToken ct)
    {
        try
        {
            var result = await _mediator.Send(request, ct);
            if (result == null) return NotFound();
            return Ok(result);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Email already exists"))
        {
            return Conflict(new { error = ex.Message });
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Role not found"))
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPut("{id:guid}/photo")]
    [Authorize(Roles = "Manager")]
    [RequestSizeLimit(10_000_000)]
    public async Task<IActionResult> UploadPhoto([FromRoute] Guid id, IFormFile file, CancellationToken ct)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file uploaded" });

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms, ct);

        try
        {
            await _mediator.Send(new UploadConsultantPhotoCommand(id, ms.ToArray()), ct);
            return NoContent();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not found"))
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
