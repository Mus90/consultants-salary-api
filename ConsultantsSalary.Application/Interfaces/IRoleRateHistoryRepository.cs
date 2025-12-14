using ConsultantsSalary.Application.Dtos;

namespace ConsultantsSalary.Application.Interfaces;

public interface IRoleRateHistoryRepository
{
    Task<List<RoleRateHistoryDto>> GetByRoleIdAsync(Guid roleId, CancellationToken ct);
    Task<RoleRateHistoryDto?> GetCurrentRateForRoleAsync(Guid roleId, CancellationToken ct);
}
