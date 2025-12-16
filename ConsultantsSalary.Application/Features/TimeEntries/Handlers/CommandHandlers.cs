using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Features.TimeEntries.Commands;
using ConsultantsSalary.Application.Interfaces;
using MediatR;

namespace ConsultantsSalary.Application.Features.TimeEntries.Handlers;

public class CreateTimeEntryHandler : IRequestHandler<CreateTimeEntryCommand, TimeEntryDto>
{
    private readonly ITimeEntryRepository _repository;
    private readonly ITimeEntryService _timeEntryService;
    private readonly IRoleRateHistoryRepository _roleRateHistoryRepository;

    public CreateTimeEntryHandler(
        ITimeEntryRepository repository,
        ITimeEntryService timeEntryService,
        IRoleRateHistoryRepository roleRateHistoryRepository)
    {
        _repository = repository;
        _timeEntryService = timeEntryService;
        _roleRateHistoryRepository = roleRateHistoryRepository;
    }

    public async Task<TimeEntryDto> Handle(CreateTimeEntryCommand request, CancellationToken cancellationToken)
    {
        var consultant = await _timeEntryService.GetConsultantAsync(request.TimeEntry.ConsultantId, cancellationToken);
        if (consultant == null)
            throw new KeyNotFoundException($"Consultant {request.TimeEntry.ConsultantId} not found");

        var currentRate = await _roleRateHistoryRepository.GetCurrentRateForRoleAsync(consultant.RoleId, cancellationToken);
        if (currentRate == null)
            throw new InvalidOperationException("No current rate found for consultant's role");

        var entity = await _timeEntryService.CreateAsync(
            request.TimeEntry.ConsultantId,
            request.TimeEntry.TaskId,
            request.TimeEntry.DateWorked,
            request.TimeEntry.HoursWorked,
            cancellationToken);

        return await _repository.GetByIdAsync(entity.Id, cancellationToken) 
            ?? throw new InvalidOperationException("Failed to retrieve created time entry");
    }
}

public class DeleteTimeEntryHandler : IRequestHandler<DeleteTimeEntryCommand, bool>
{
    private readonly ITimeEntryRepository _repository;

    public DeleteTimeEntryHandler(ITimeEntryRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteTimeEntryCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.Id, cancellationToken);
    }
}
