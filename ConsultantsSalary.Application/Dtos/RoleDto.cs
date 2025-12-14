namespace ConsultantsSalary.Application.Dtos;

public class RoleDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal? CurrentRatePerHour { get; set; }
}

public class UpdateRoleRateDto
{
    public decimal RatePerHour { get; set; }
}
