using ConsultantsSalary.Domain.Entities;

namespace ConsultantsSalary.Application.Interfaces;

public interface ITimeEntryService
{
    Task<TimeEntry> CreateAsync(Guid consultantId, Guid taskId, DateTime dateWorked, decimal hoursWorked, CancellationToken ct = default);
    Task<Consultant?> GetConsultantAsync(Guid consultantId, CancellationToken ct = default);
}
