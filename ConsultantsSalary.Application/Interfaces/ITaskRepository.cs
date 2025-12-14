using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace ConsultantsSalary.Application.Interfaces;

public interface ITaskRepository
{
    Task<List<TaskDto>> GetAllAsync(CancellationToken ct);
    Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<TaskDto> CreateAsync(CreateTaskDto dto, CancellationToken ct);
    Task<TaskDto?> UpdateAsync(Guid id, UpdateTaskDto dto, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    Task<bool> TaskExistsAsync(Guid id, CancellationToken ct);
    Task<bool> ConsultantExistsAsync(Guid consultantId, CancellationToken ct);
    Task AssignConsultantAsync(Guid taskId, Guid consultantId, CancellationToken ct);
    Task UnassignConsultantAsync(Guid taskId, Guid consultantId, CancellationToken ct);
}
