using ConsultantsSalary.Domain.Entities;

namespace ConsultantsSalary.Application.Interfaces;
using System.Threading.Tasks;

public interface IRateHistoryService
{
    Task<RoleRateHistory> SetNewRateAsync(Guid roleId, decimal newRatePerHour, DateTime effectiveUtc, CancellationToken ct = default);
    Task<RoleRateHistory?> GetCurrentRateAsync(Guid roleId, CancellationToken ct = default);
    Task SetNewRoleRateAsync(Guid roleId, decimal ratePerHour, CancellationToken cancellationToken);
}
