namespace ConsultantsSalary.Domain.Entities;

public class Consultant
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Current Role assignment
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public byte[]? ProfileImage { get; set; }

    public ICollection<ConsultantTaskAssignment> TaskAssignments { get; set; } = new List<ConsultantTaskAssignment>();
    public ICollection<TimeEntry> TimeEntries { get; set; } = new List<TimeEntry>();
}
