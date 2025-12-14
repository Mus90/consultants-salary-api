using ConsultantsSalary.Application.Dtos;
using MediatR;

namespace ConsultantsSalary.Application.Features.Roles.Commands;

public record UpdateRoleRateCommand(Guid RoleId, UpdateRoleRateDto RateUpdate) : IRequest<RoleDto>;
