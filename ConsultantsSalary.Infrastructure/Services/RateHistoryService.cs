using ConsultantsSalary.Application.Interfaces;
using ConsultantsSalary.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ConsultantsSalary.Infrastructure.Services;

public class RateHistoryService : IRateHistoryService
{
    private readonly AppDbContext _db;

    public RateHistoryService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<RoleRateHistory> SetNewRateAsync(Guid roleId, decimal newRatePerHour, DateTime effectiveUtc, CancellationToken ct = default)
    {
        if (newRatePerHour <= 0) throw new ArgumentOutOfRangeException(nameof(newRatePerHour));

        var roleExists = await _db.Set<Role>().AnyAsync(r => r.Id == roleId, ct);
        if (!roleExists) throw new KeyNotFoundException($"Role {roleId} not found");

        var current = await _db.RoleRateHistories
            .Where(h => h.RoleId == roleId)
            .OrderByDescending(h => h.EffectiveDate)
            .FirstOrDefaultAsync(ct);

        if (current != null)
        {
            if (current.EndDate is null || current.EndDate > effectiveUtc)
            {
                current.EndDate = effectiveUtc;
            }
        }

        var newEntry = new RoleRateHistory
        {
            Id = Guid.NewGuid(),
            RoleId = roleId,
            RatePerHour = newRatePerHour,
            EffectiveDate = effectiveUtc,
            EndDate = null
        };

        _db.RoleRateHistories.Add(newEntry);
        await _db.SaveChangesAsync(ct);
        return newEntry;
    }

    public async Task<RoleRateHistory?> GetCurrentRateAsync(Guid roleId, CancellationToken ct = default)
    {
        var currentRate = await _db.RoleRateHistories
            .Include(rh => rh.Role)
            .Where(rh => rh.RoleId == roleId && rh.EndDate == null)
            .OrderByDescending(rh => rh.EffectiveDate)
            .FirstOrDefaultAsync(ct);

        return currentRate;
    }

    public async System.Threading.Tasks.Task SetNewRoleRateAsync(Guid roleId, decimal ratePerHour, CancellationToken cancellationToken)
    {
        await SetNewRateAsync(roleId, ratePerHour, DateTime.UtcNow, cancellationToken);
    }

}
