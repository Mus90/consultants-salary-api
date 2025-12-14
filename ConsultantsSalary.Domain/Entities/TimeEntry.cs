namespace ConsultantsSalary.Domain.Entities;

public class TimeEntry
{
    public Guid Id { get; set; }
    public Guid ConsultantId { get; set; }
    public Consultant Consultant { get; set; } = null!;

    public Guid TaskId { get; set; }
    public Task Task { get; set; } = null!;

    public DateTime DateWorked { get; set; }
    public decimal HoursWorked { get; set; }

    public Guid RateSnapshotId { get; set; }
    public RoleRateHistory RateSnapshot { get; set; } = null!;
}
