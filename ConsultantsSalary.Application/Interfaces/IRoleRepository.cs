using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace ConsultantsSalary.Application.Interfaces;

public interface IRoleRepository
{
    Task<List<RoleDto>> GetAllAsync(CancellationToken ct);
    Task<RoleDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<RoleDto>> GetAllWithCurrentRatesAsync(CancellationToken ct);
    Task<RoleDto?> GetByIdWithCurrentRateAsync(Guid id, CancellationToken ct);
    Task<bool> RoleExistsAsync(Guid id, CancellationToken ct);
}
