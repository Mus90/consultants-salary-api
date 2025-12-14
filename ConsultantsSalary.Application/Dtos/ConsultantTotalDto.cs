namespace ConsultantsSalary.Application.Dtos;

public class ConsultantTotalDto
{
    public Guid ConsultantId { get; set; }
    public string ConsultantName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal TotalHoursWorked { get; set; }
    public decimal TotalAmountDue { get; set; }
    public int TimeEntryCount { get; set; }
    public List<TimeEntrySummaryDto> TimeEntries { get; set; } = new();
}

public class TimeEntrySummaryDto
{
    public DateTime DateWorked { get; set; }
    public decimal HoursWorked { get; set; }
    public decimal HourlyRate { get; set; }
    public decimal Amount { get; set; }
    public string TaskName { get; set; } = string.Empty;
}
