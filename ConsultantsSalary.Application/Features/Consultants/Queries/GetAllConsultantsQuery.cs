using ConsultantsSalary.Application.Interfaces;
using MediatR;

namespace ConsultantsSalary.Application.Features.Consultants.Queries;

public record GetAllConsultantsQuery : IRequest<List<ConsultantDto>>;
public class ConsultantDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public byte[]? ProfileImage { get; set; }
}

public class GetAllConsultantsHandler : IRequestHandler<GetAllConsultantsQuery, List<ConsultantDto>>
{
    private readonly IConsultantRepository _repository;

    public GetAllConsultantsHandler(IConsultantRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ConsultantDto>> Handle(GetAllConsultantsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}
