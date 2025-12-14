using ConsultantsSalary.Application.Dtos;
using MediatR;

namespace ConsultantsSalary.Application.Features.Roles.Queries;

public record GetAllRolesQuery : IRequest<List<RoleDto>>;

public record GetAllRolesWithCurrentRatesQuery : IRequest<List<RoleDto>>;

public record GetRoleByIdQuery(Guid Id) : IRequest<RoleDto?>;

public record GetRoleByIdWithCurrentRateQuery(Guid Id) : IRequest<RoleDto?>;
