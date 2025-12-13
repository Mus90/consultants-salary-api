namespace ConsultantsSalary.Domain.Entities;

public class Task
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public ICollection<ConsultantTaskAssignment> Assignments { get; set; } = new List<ConsultantTaskAssignment>();
    public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
}
