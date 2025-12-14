using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Features.TimeEntries.Queries;
using ConsultantsSalary.Application.Interfaces;
using MediatR;
using Task = System.Threading.Tasks.Task;

namespace ConsultantsSalary.Application.Features.TimeEntries.Handlers;

public class GetAllTimeEntriesHandler : IRequestHandler<GetAllTimeEntriesQuery, List<TimeEntryDto>>
{
    private readonly ITimeEntryRepository _repository;

    public GetAllTimeEntriesHandler(ITimeEntryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TimeEntryDto>> Handle(GetAllTimeEntriesQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}

public class GetTimeEntriesByConsultantIdHandler : IRequestHandler<GetTimeEntriesByConsultantIdQuery, List<TimeEntryDto>>
{
    private readonly ITimeEntryRepository _repository;

    public GetTimeEntriesByConsultantIdHandler(ITimeEntryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TimeEntryDto>> Handle(GetTimeEntriesByConsultantIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByConsultantIdAsync(request.ConsultantId, cancellationToken);
    }
}

public class GetTimeEntriesByTaskIdHandler : IRequestHandler<GetTimeEntriesByTaskIdQuery, List<TimeEntryDto>>
{
    private readonly ITimeEntryRepository _repository;

    public GetTimeEntriesByTaskIdHandler(ITimeEntryRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TimeEntryDto>> Handle(GetTimeEntriesByTaskIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByTaskIdAsync(request.TaskId, cancellationToken);
    }
}

public class GetTimeEntryByIdHandler : IRequestHandler<GetTimeEntryByIdQuery, TimeEntryDto?>
{
    private readonly ITimeEntryRepository _repository;

    public GetTimeEntryByIdHandler(ITimeEntryRepository repository)
    {
        _repository = repository;
    }

    public async Task<TimeEntryDto?> Handle(GetTimeEntryByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id, cancellationToken);
    }
}
