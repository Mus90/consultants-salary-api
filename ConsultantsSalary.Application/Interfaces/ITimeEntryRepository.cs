using ConsultantsSalary.Application.Dtos;
using Task = System.Threading.Tasks.Task;

namespace ConsultantsSalary.Application.Interfaces;

public interface ITimeEntryRepository
{
    Task<List<TimeEntryDto>> GetAllAsync(CancellationToken ct);
    Task<List<TimeEntryDto>> GetByConsultantIdAsync(Guid consultantId, CancellationToken ct);
    Task<List<TimeEntryDto>> GetByTaskIdAsync(Guid taskId, CancellationToken ct);
    Task<TimeEntryDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<TimeEntryDto> CreateAsync(CreateTimeEntryDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}
