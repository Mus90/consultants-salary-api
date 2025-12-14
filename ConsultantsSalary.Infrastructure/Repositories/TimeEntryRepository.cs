using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Interfaces;
using ConsultantsSalary.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Mapster;
using Task = System.Threading.Tasks.Task;

namespace ConsultantsSalary.Infrastructure.Repositories;

public class TimeEntryRepository : ITimeEntryRepository
{
    private readonly AppDbContext _db;

    public TimeEntryRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<TimeEntryDto>> GetAllAsync(CancellationToken ct)
    {
        var timeEntries = await _db.TimeEntries
            .Include(te => te.Consultant)
            .Include(te => te.Task)
            .ToListAsync(ct);

        return timeEntries.Adapt<List<TimeEntryDto>>();
    }

    public async Task<List<TimeEntryDto>> GetByConsultantIdAsync(Guid consultantId, CancellationToken ct)
    {
        var timeEntries = await _db.TimeEntries
            .Include(te => te.Consultant)
            .Include(te => te.Task)
            .Where(te => te.ConsultantId == consultantId)
            .ToListAsync(ct);

        return timeEntries.Adapt<List<TimeEntryDto>>();
    }

    public async Task<List<TimeEntryDto>> GetByTaskIdAsync(Guid taskId, CancellationToken ct)
    {
        var timeEntries = await _db.TimeEntries
            .Include(te => te.Consultant)
            .Include(te => te.Task)
            .Where(te => te.TaskId == taskId)
            .ToListAsync(ct);

        return timeEntries.Adapt<List<TimeEntryDto>>();
    }

    public async Task<TimeEntryDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var timeEntry = await _db.TimeEntries
            .Include(te => te.Consultant)
            .Include(te => te.Task)
            .FirstOrDefaultAsync(te => te.Id == id, ct);

        return timeEntry?.Adapt<TimeEntryDto>();
    }

    public async Task<TimeEntryDto> CreateAsync(CreateTimeEntryDto dto, CancellationToken ct)
    {
        var entity = dto.Adapt<Domain.Entities.TimeEntry>();
        entity.Id = Guid.NewGuid();

        _db.TimeEntries.Add(entity);
        await _db.SaveChangesAsync(ct);

        return await GetByIdAsync(entity.Id, ct) ?? throw new InvalidOperationException("Failed to retrieve created time entry");
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await _db.TimeEntries.FirstOrDefaultAsync(te => te.Id == id, ct);
        if (entity == null) return false;

        _db.TimeEntries.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
