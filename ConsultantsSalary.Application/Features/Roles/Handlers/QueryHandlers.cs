using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Features.Roles.Queries;
using ConsultantsSalary.Application.Interfaces;
using MediatR;
using Task = System.Threading.Tasks.Task;

namespace ConsultantsSalary.Application.Features.Roles.Handlers;

public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
{
    private readonly IRoleRepository _repository;

    public GetAllRolesHandler(IRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}

public class GetAllRolesWithCurrentRatesHandler : IRequestHandler<GetAllRolesWithCurrentRatesQuery, List<RoleDto>>
{
    private readonly IRoleRepository _repository;

    public GetAllRolesWithCurrentRatesHandler(IRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<RoleDto>> Handle(GetAllRolesWithCurrentRatesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllWithCurrentRatesAsync(cancellationToken);
    }
}

public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly IRoleRepository _repository;

    public GetRoleByIdHandler(IRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<RoleDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id, cancellationToken);
    }
}

public class GetRoleByIdWithCurrentRateHandler : IRequestHandler<GetRoleByIdWithCurrentRateQuery, RoleDto?>
{
    private readonly IRoleRepository _repository;

    public GetRoleByIdWithCurrentRateHandler(IRoleRepository repository)
    {
        _repository = repository;
    }

    public async Task<RoleDto?> Handle(GetRoleByIdWithCurrentRateQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdWithCurrentRateAsync(request.Id, cancellationToken);
    }
}
