namespace ConsultantsSalary.Application.Dtos;

public class TimeEntryDto
{
    public Guid Id { get; set; }
    public Guid ConsultantId { get; set; }
    public string ConsultantName { get; set; } = string.Empty;
    public Guid TaskId { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public DateTime DateWorked { get; set; }
    public decimal HoursWorked { get; set; }
    public decimal RatePerHour { get; set; }
    public decimal TotalAmount => HoursWorked * RatePerHour;
}

public class CreateTimeEntryDto
{
    public Guid RateSnapshotId { get; set; }
    public Guid ConsultantId { get; set; }
    public Guid TaskId { get; set; }
    public DateTime DateWorked { get; set; }
    public decimal HoursWorked { get; set; }
}
