namespace ConsultantsSalary.Domain.Entities;

public class RoleRateHistory
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public Role Role { get; set; } = null!;

    public decimal RatePerHour { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
}
