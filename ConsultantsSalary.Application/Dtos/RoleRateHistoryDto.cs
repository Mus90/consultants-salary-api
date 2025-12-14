namespace ConsultantsSalary.Application.Dtos;

public class RoleRateHistoryDto
{
    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public decimal RatePerHour { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class CreateRoleRateHistoryDto
{
    public Guid RoleId { get; set; }
    public decimal RatePerHour { get; set; }
    public DateTime EffectiveDate { get; set; }
}
