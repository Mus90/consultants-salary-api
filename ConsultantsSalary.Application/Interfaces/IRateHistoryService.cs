using ConsultantsSalary.Domain.Entities;

namespace ConsultantsSalary.Application.Interfaces;
using System.Threading.Tasks;

public interface IRateHistoryService
{
    Task<RoleRateHistory> SetNewRateAsync(Guid roleId, decimal newRatePerHour, DateTime effectiveUtc, CancellationToken ct = default);
    Task<RoleRateHistory?> GetRateForDateAsync(Guid roleId, DateTime dateWorked, CancellationToken ct = default);
    Task SetNewRoleRateAsync(Guid roleId, decimal ratePerHour, CancellationToken cancellationToken);
}
