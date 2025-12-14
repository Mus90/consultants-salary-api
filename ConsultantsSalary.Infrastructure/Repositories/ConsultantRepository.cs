using ConsultantsSalary.Application.Features.Consultants.Commands;
using ConsultantsSalary.Application.Features.Consultants.Queries;
using ConsultantsSalary.Application.Interfaces;
using ConsultantsSalary.Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace ConsultantsSalary.Infrastructure.Repositories;

public class ConsultantRepository : IConsultantRepository
{
    private readonly AppDbContext _db;

    public ConsultantRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ConsultantDto>> GetAllAsync(CancellationToken ct)
    {
        var consultants = await _db.Consultants
            .Include(c => c.Role)
            .ToListAsync(ct);

        return consultants.Adapt<List<ConsultantDto>>();
    }

    public async Task<ConsultantDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var consultant = await _db.Consultants
            .Include(c => c.Role)
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        return consultant?.Adapt<ConsultantDto>();
    }

    public async Task<ConsultantDto> CreateAsync(CreateConsultantCommand dto, CancellationToken ct)
    {
        var entity = dto.Adapt<Consultant>();
        entity.Id = Guid.NewGuid();

        _db.Consultants.Add(entity);
        await _db.SaveChangesAsync(ct);

        return await GetByIdAsync(entity.Id, ct) ?? throw new InvalidOperationException("Failed to retrieve created consultant");
    }

    public async Task<ConsultantDto?> UpdateAsync(Guid id, UpdateConsultantCommand dto, CancellationToken ct)
    {
        var entity = await _db.Consultants.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity == null) return null;

        dto.Adapt(entity);
        await _db.SaveChangesAsync(ct);

        return await GetByIdAsync(id, ct);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var entity = await _db.Consultants.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity == null) return false;

        _db.Consultants.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeId = null, CancellationToken ct = default)
    {
        var query = _db.Consultants.Where(c => c.Email == email);
        if (excludeId.HasValue)
            query = query.Where(c => c.Id != excludeId.Value);

        return await query.AnyAsync(ct);
    }

    public async Task<bool> RoleExistsAsync(Guid roleId, CancellationToken ct)
    {
        return await _db.ConsultantRoles.AnyAsync(r => r.Id == roleId, ct);
    }

    public async Task<byte[]?> GetProfileImageAsync(Guid id, CancellationToken ct)
    {
        var consultant = await _db.Consultants
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);

        return consultant?.ProfileImage;
    }

    public async Task UpdateProfileImageAsync(Guid id, byte[] imageBytes, CancellationToken ct)
    {
        var entity = await _db.Consultants.FirstOrDefaultAsync(c => c.Id == id, ct);
        if (entity == null) throw new InvalidOperationException("Consultant not found");

        entity.ProfileImage = imageBytes;
        await _db.SaveChangesAsync(ct);
    }
}
