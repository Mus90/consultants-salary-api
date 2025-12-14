using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Exceptions;
using ConsultantsSalary.Application.Features.TimeEntries.Commands;
using ConsultantsSalary.Application.Features.TimeEntries.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsultantsSalary.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class TimeEntriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public TimeEntriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<TimeEntryDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllTimeEntriesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("consultant/{consultantId:guid}")]
    public async Task<ActionResult<List<TimeEntryDto>>> GetByConsultant(Guid consultantId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTimeEntriesByConsultantIdQuery(consultantId), ct);
        return Ok(result);
    }

    [HttpGet("task/{taskId:guid}")]
    public async Task<ActionResult<List<TimeEntryDto>>> GetByTask(Guid taskId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTimeEntriesByTaskIdQuery(taskId), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TimeEntryDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTimeEntryByIdQuery(id), ct);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("consultant/{consultantId:guid}/total")]
    public async Task<ActionResult<ConsultantTotalDto>> GetConsultantTotal(
        Guid consultantId, 
        [FromQuery] DateTime startDate, 
        [FromQuery] DateTime endDate, 
        CancellationToken ct)
    {
        if (startDate > endDate)
            return BadRequest(new { error = "Start date cannot be after end date" });

        var result = await _mediator.Send(new GetConsultantTotalQuery(consultantId, startDate, endDate), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TimeEntryDto>> Create([FromBody] CreateTimeEntryDto request, CancellationToken ct)
    {
        try
        {
            var result = await _mediator.Send(new CreateTimeEntryCommand(request), ct);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (DailyLimitExceededException ex)
        {
            return BadRequest(new { error = ex.Message, ex.ConsultantId, ex.DateWorked, ex.AttemptedTotalHours });
        }
        catch (ArgumentOutOfRangeException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteTimeEntryCommand(id), ct);
        if (!result) return NotFound();
        return NoContent();
    }
}
