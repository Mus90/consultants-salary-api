using ConsultantsSalary.Application.Exceptions;
using ConsultantsSalary.Application.Interfaces;
using ConsultantsSalary.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsultantsSalary.Infrastructure.Services;

public class TimeEntryService : ITimeEntryService
{
    private readonly AppDbContext _db;
    private readonly IRateHistoryService _rateHistory;

    public TimeEntryService(AppDbContext db, IRateHistoryService rateHistory)
    {
        _db = db;
        _rateHistory = rateHistory;
    }

    public async Task<TimeEntry> CreateAsync(Guid consultantId, Guid taskId, DateTime dateWorked, decimal hoursWorked, CancellationToken ct = default)
    {
        if (hoursWorked <= 0) throw new ArgumentOutOfRangeException(nameof(hoursWorked));

        var consultant = await _db.Consultants.FirstOrDefaultAsync(c => c.Id == consultantId, ct)
            ?? throw new KeyNotFoundException($"Consultant {consultantId} not found");

        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == taskId, ct)
            ?? throw new KeyNotFoundException($"Task {taskId} not found");

        var assigned = await _db.ConsultantTaskAssignments.AnyAsync(a => a.ConsultantId == consultantId && a.TaskId == taskId, ct);
        if (!assigned)
            throw new InvalidOperationException("Consultant is not assigned to the specified task.");

        var existingTotal = await _db.TimeEntries
            .Where(te => te.ConsultantId == consultantId && te.DateWorked == dateWorked)
            .SumAsync(te => (decimal?)te.HoursWorked, ct) ?? 0m;

        var attempted = existingTotal + hoursWorked;
        if (attempted > 12m)
            throw new DailyLimitExceededException(consultantId, dateWorked, attempted);

        var snapshot = await _rateHistory.GetCurrentRateAsync(consultant.RoleId, ct)
            ?? throw new InvalidOperationException("No rate found for consultant's role on the specified date.");

        var entity = new TimeEntry
        {
            Id = Guid.NewGuid(),
            ConsultantId = consultantId,
            TaskId = taskId,
            DateWorked = dateWorked,
            HoursWorked = hoursWorked,
            RateSnapshotId = snapshot.Id
        };

        _db.TimeEntries.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<Consultant?> GetConsultantAsync(Guid consultantId, CancellationToken ct = default)
    {
        return await _db.Consultants
            .Include(c => c.Role)
            .FirstOrDefaultAsync(c => c.Id == consultantId, ct);
    }
}
