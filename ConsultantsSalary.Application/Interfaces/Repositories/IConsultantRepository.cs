using ConsultantsSalary.Domain.Entities;
using Task = System.Threading.Tasks.Task;

namespace ConsultantsSalary.Application.Interfaces.Repositories;

public interface IConsultantRepository
{
    Task<List<Consultant>> GetAllAsync(CancellationToken ct);
    Task<Consultant?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> EmailExistsAsync(string email, Guid? exceptId, CancellationToken ct);
    Task AddAsync(Consultant consultant, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
