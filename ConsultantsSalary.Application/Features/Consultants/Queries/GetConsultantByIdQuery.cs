using ConsultantsSalary.Application.Interfaces;
using MediatR;

namespace ConsultantsSalary.Application.Features.Consultants.Queries;

public record GetConsultantByIdQuery(Guid Id) : IRequest<ConsultantDto?>;

public class GetConsultantByIdHandler : IRequestHandler<GetConsultantByIdQuery, ConsultantDto?>
{
    private readonly IConsultantRepository _repository;

    public GetConsultantByIdHandler(IConsultantRepository repository)
    {
        _repository = repository;
    }

    public async Task<ConsultantDto?> Handle(GetConsultantByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id, cancellationToken);
    }
}
