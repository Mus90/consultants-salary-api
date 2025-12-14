using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Features.Roles.Commands;
using ConsultantsSalary.Application.Interfaces;
using MediatR;
using Task = System.Threading.Tasks.Task;

namespace ConsultantsSalary.Application.Features.Roles.Handlers;

public class UpdateRoleRateHandler : IRequestHandler<UpdateRoleRateCommand, RoleDto>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IRateHistoryService _rateHistoryService;

    public UpdateRoleRateHandler(IRoleRepository roleRepository, IRateHistoryService rateHistoryService)
    {
        _roleRepository = roleRepository;
        _rateHistoryService = rateHistoryService;
    }

    public async Task<RoleDto> Handle(UpdateRoleRateCommand request, CancellationToken cancellationToken)
    {
        if (!await _roleRepository.RoleExistsAsync(request.RoleId, cancellationToken))
            throw new InvalidOperationException("Role not found");

        await _rateHistoryService.SetNewRoleRateAsync(request.RoleId, request.RateUpdate.RatePerHour, cancellationToken);

        var result = await _roleRepository.GetByIdWithCurrentRateAsync(request.RoleId, cancellationToken);
        return result ?? throw new InvalidOperationException("Failed to retrieve updated role");
    }
}
