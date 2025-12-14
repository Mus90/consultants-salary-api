namespace ConsultantsSalary.Domain.Entities;

public class ConsultantTaskAssignment
{
    public Guid Id { get; set; }
    public Guid ConsultantId { get; set; }
    public Consultant Consultant { get; set; } = null!;

    public Guid TaskId { get; set; }
    public Task Task { get; set; } = null!;

    public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
}
