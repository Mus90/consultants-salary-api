using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ConsultantsSalary.Infrastructure.Repositories;

public class RoleRateHistoryRepository : IRoleRateHistoryRepository
{
    private readonly AppDbContext _db;

    public RoleRateHistoryRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<RoleRateHistoryDto>> GetByRoleIdAsync(Guid roleId, CancellationToken ct)
    {
        var rateHistories = await _db.RoleRateHistories
            .Include(rh => rh.Role)
            .Where(rh => rh.RoleId == roleId)
            .OrderByDescending(rh => rh.EffectiveDate)
            .ToListAsync(ct);

        return rateHistories.Adapt<List<RoleRateHistoryDto>>();
    }

    public async Task<RoleRateHistoryDto?> GetCurrentRateForRoleAsync(Guid roleId, CancellationToken ct)
    {
        var currentRate = await _db.RoleRateHistories
            .Include(rh => rh.Role)
            .Where(rh => rh.RoleId == roleId && rh.EndDate == null)
            .OrderByDescending(rh => rh.EffectiveDate)
            .FirstOrDefaultAsync(ct);

        return currentRate?.Adapt<RoleRateHistoryDto>();
    }
}
