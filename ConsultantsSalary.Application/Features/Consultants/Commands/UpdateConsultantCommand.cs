using ConsultantsSalary.Application.Features.Consultants.Queries;
using ConsultantsSalary.Application.Interfaces;
using MediatR;

namespace ConsultantsSalary.Application.Features.Consultants.Commands;

public class UpdateConsultantCommand : IRequest<ConsultantDto?>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
}

public class UpdateConsultantHandler : IRequestHandler<UpdateConsultantCommand, ConsultantDto?>
{
    private readonly IConsultantRepository _repository;

    public UpdateConsultantHandler(IConsultantRepository repository)
    {
        _repository = repository;
    }

    public async Task<ConsultantDto?> Handle(UpdateConsultantCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.RoleExistsAsync(request.RoleId, cancellationToken))
            throw new InvalidOperationException("Role not found");

        if (await _repository.EmailExistsAsync(request.Email, request.Id, cancellationToken))
            throw new InvalidOperationException("Email already exists");

        return await _repository.UpdateAsync(request.Id, request, cancellationToken);
    }
}
