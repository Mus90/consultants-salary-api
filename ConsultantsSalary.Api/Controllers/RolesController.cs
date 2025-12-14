using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Features.Roles.Commands;
using ConsultantsSalary.Application.Features.Roles.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsultantsSalary.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<RoleDto>>> GetRoles(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllRolesWithCurrentRatesQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<RoleDto>> GetRole(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetRoleByIdWithCurrentRateQuery(id), ct);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("{id:guid}/rate")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<RoleDto>> UpdateRate([FromRoute] Guid id, [FromBody] UpdateRoleRateDto request, CancellationToken ct)
    {
        try
        {
            var result = await _mediator.Send(new UpdateRoleRateCommand(id, request), ct);
            return Ok(result);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Role not found"))
        {
            return NotFound(new { error = ex.Message });
        }
    }
}
