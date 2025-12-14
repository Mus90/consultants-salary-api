using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Interfaces;
using ConsultantsSalary.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace ConsultantsSalary.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;

    public TaskRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<TaskDto>> GetAllAsync(CancellationToken ct)
    {
        var tasks = await _db.Tasks.ToListAsync(ct);
        return tasks.Adapt<List<TaskDto>>();
    }

    public async Task<TaskDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);
        return task?.Adapt<TaskDto>();
    }

    public async Task<TaskDto> CreateAsync(CreateTaskDto dto, CancellationToken ct)
    {
        var entity = dto.Adapt<Domain.Entities.Task>();
        entity.Id = Guid.NewGuid();

        _db.Tasks.Add(entity);
        await _db.SaveChangesAsync(ct);

        return await GetByIdAsync(entity.Id, ct) ?? throw new InvalidOperationException("Failed to retrieve created task");
    }

    public async Task<TaskDto?> UpdateAsync(Guid id, UpdateTaskDto dto, CancellationToken ct)
    {
        var entity = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (entity == null) return null;

        dto.Adapt(entity);
        await _db.SaveChangesAsync(ct);

        return await GetByIdAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id, ct);
        if (entity == null) return false;

        _db.Tasks.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> TaskExistsAsync(Guid id, CancellationToken ct)
    {
        return await _db.Tasks.AnyAsync(t => t.Id == id, ct);
    }

    public async Task<bool> ConsultantExistsAsync(Guid consultantId, CancellationToken ct)
    {
        return await _db.Consultants.AnyAsync(c => c.Id == consultantId, ct);
    }

    public async Task AssignConsultantAsync(Guid taskId, Guid consultantId, CancellationToken ct)
    {
        var existingAssignment = await _db.ConsultantTaskAssignments
            .FirstOrDefaultAsync(cta => cta.TaskId == taskId && cta.ConsultantId == consultantId, ct);

        if (existingAssignment != null) return;

        var assignment = new ConsultantTaskAssignment
        {
            Id = Guid.NewGuid(),
            ConsultantId = consultantId,
            TaskId = taskId,
        };

        _db.ConsultantTaskAssignments.Add(assignment);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UnassignConsultantAsync(Guid taskId, Guid consultantId, CancellationToken ct)
    {
        var assignment = await _db.ConsultantTaskAssignments
            .FirstOrDefaultAsync(cta => cta.TaskId == taskId && cta.ConsultantId == consultantId, ct);

        if (assignment == null) return;

        _db.ConsultantTaskAssignments.Remove(assignment);
        await _db.SaveChangesAsync(ct);
    }
}
