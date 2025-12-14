using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ConsultantsSalary.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _db;

    public RoleRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<RoleDto>> GetAllAsync(CancellationToken ct)
    {
        var roles = await _db.ConsultantRoles.ToListAsync(ct);
        return roles.Adapt<List<RoleDto>>();
    }

    public async Task<RoleDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var role = await _db.ConsultantRoles.FirstOrDefaultAsync(r => r.Id == id, ct);
        return role?.Adapt<RoleDto>();
    }

    public async Task<List<RoleDto>> GetAllWithCurrentRatesAsync(CancellationToken ct)
    {
        var roles = await _db.ConsultantRoles
            .Include(r => r.RateHistory)
            .ToListAsync(ct);

        var roleDtos = new List<RoleDto>();
        foreach (var role in roles)
        {
            var currentRate = role.RateHistory
                .Where(rh => rh.EndDate == null)
                .OrderByDescending(rh => rh.EffectiveDate)
                .FirstOrDefault();

            roleDtos.Add(new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
                CurrentRatePerHour = currentRate?.RatePerHour
            });
        }

        return roleDtos;
    }

    public async Task<RoleDto?> GetByIdWithCurrentRateAsync(Guid id, CancellationToken ct)
    {
        var role = await _db.ConsultantRoles
            .Include(r => r.RateHistory)
            .FirstOrDefaultAsync(r => r.Id == id, ct);

        if (role == null) return null;

        var currentRate = role.RateHistory
            .Where(rh => rh.EndDate == null)
            .OrderByDescending(rh => rh.EffectiveDate)
            .FirstOrDefault();

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            CurrentRatePerHour = currentRate?.RatePerHour
        };
    }

    public async Task<bool> RoleExistsAsync(Guid id, CancellationToken ct)
    {
        return await _db.ConsultantRoles.AnyAsync(r => r.Id == id, ct);
    }
}
