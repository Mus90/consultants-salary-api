namespace ConsultantsSalary.Application.Dtos;

public class TaskHoursDto
{
    public Guid TaskId { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public Guid ConsultantId { get; set; }
    public string ConsultantName { get; set; } = string.Empty;
    public decimal TotalHoursWorked { get; set; }
    public decimal TotalAmountDue { get; set; }
    public int TimeEntryCount { get; set; }
    public List<TaskTimeEntrySummaryDto> TimeEntries { get; set; } = new();
}

public class TaskTimeEntrySummaryDto
{
    public DateTime DateWorked { get; set; }
    public decimal HoursWorked { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal Amount { get; set; }
}
