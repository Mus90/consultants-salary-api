namespace ConsultantsSalary.Domain.Entities;

public class Role
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public ICollection<RoleRateHistory> RateHistory { get; set; } = new List<RoleRateHistory>();
    public ICollection<Consultant> Consultants { get; set; } = new List<Consultant>();
}
