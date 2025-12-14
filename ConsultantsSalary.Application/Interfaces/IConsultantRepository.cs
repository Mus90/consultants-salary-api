using ConsultantsSalary.Application.Features.Consultants.Commands;
using ConsultantsSalary.Application.Features.Consultants.Queries;
using Task = System.Threading.Tasks.Task;

namespace ConsultantsSalary.Application.Interfaces;

public interface IConsultantRepository
{
    Task<List<ConsultantDto>> GetAllAsync(CancellationToken ct);
    Task<ConsultantDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<ConsultantDto> CreateAsync(CreateConsultantCommand dto, CancellationToken ct);
    Task<ConsultantDto?> UpdateAsync(Guid id, UpdateConsultantCommand dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    Task<bool> EmailExistsAsync(string email, Guid? excludeId = null, CancellationToken ct = default);
    Task<bool> RoleExistsAsync(Guid roleId, CancellationToken ct);
    Task<byte[]?> GetProfileImageAsync(Guid id, CancellationToken ct);
    Task UpdateProfileImageAsync(Guid id, byte[] imageBytes, CancellationToken ct);
}
