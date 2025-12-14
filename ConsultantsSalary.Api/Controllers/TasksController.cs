using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Features.Tasks.Commands;
using ConsultantsSalary.Application.Features.Tasks.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsultantsSalary.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<TaskDto>>> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllTasksQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<TaskDto>> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetTaskByIdQuery(id), ct);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<TaskDto>> Create([FromBody] CreateTaskDto request, CancellationToken ct)
    {
        var result = await _mediator.Send(new CreateTaskCommand(request), ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<TaskDto>> Update([FromRoute] Guid id, [FromBody] UpdateTaskDto request, CancellationToken ct)
    {
        try
        {
            var result = await _mediator.Send(new UpdateTaskCommand(id, request), ct);
            if (result == null) return NotFound();
            return Ok(result);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Task not found"))
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new DeleteTaskCommand(id), ct);
        if (!result) return NotFound();
        return NoContent();
    }

    public record AssignRequest(Guid ConsultantId);

    [HttpPost("{taskId:guid}/assign")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Assign([FromRoute] Guid taskId, [FromBody] AssignRequest request, CancellationToken ct)
    {
        try
        {
            await _mediator.Send(new AssignConsultantToTaskCommand(taskId, request.ConsultantId), ct);
            return NoContent();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not found"))
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpPost("{taskId:guid}/unassign")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Unassign([FromRoute] Guid taskId, [FromBody] AssignRequest request, CancellationToken ct)
    {
        try
        {
            await _mediator.Send(new UnassignConsultantFromTaskCommand(taskId, request.ConsultantId), ct);
            return NoContent();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not found"))
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
