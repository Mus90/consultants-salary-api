using ConsultantsSalary.Application.Features.Consultants.Queries;
using ConsultantsSalary.Application.Interfaces;
using MediatR;

namespace ConsultantsSalary.Application.Features.Consultants.Commands;

public class CreateConsultantCommand : IRequest<ConsultantDto>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
}

public class CreateConsultantHandler : IRequestHandler<CreateConsultantCommand, ConsultantDto>
{
    private readonly IConsultantRepository _repository;

    public CreateConsultantHandler(IConsultantRepository repository)
    {
        _repository = repository;
    }

    public async Task<ConsultantDto> Handle(CreateConsultantCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.EmailExistsAsync(request.Email, null, cancellationToken))
            throw new InvalidOperationException("Email already exists");

        if (!await _repository.RoleExistsAsync(request.RoleId, cancellationToken))
            throw new InvalidOperationException("Role not found");

        return await _repository.CreateAsync(request, cancellationToken);
    }
}