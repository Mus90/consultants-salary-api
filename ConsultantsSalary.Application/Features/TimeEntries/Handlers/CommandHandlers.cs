using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Features.TimeEntries.Commands;
using ConsultantsSalary.Application.Interfaces;
using MediatR;

namespace ConsultantsSalary.Application.Features.TimeEntries.Handlers;

public class CreateTimeEntryHandler : IRequestHandler<CreateTimeEntryCommand, TimeEntryDto>
{
    private readonly ITimeEntryRepository _repository;
    private readonly ITimeEntryService _timeEntryService;

    public CreateTimeEntryHandler(ITimeEntryRepository repository, ITimeEntryService timeEntryService)
    {
        _repository = repository;
        _timeEntryService = timeEntryService;
    }

    public async Task<TimeEntryDto> Handle(CreateTimeEntryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _timeEntryService.CreateAsync(
            request.TimeEntry.ConsultantId,
            request.TimeEntry.TaskId,
            request.TimeEntry.DateWorked,
            request.TimeEntry.HoursWorked,
            cancellationToken);

        var dto = new CreateTimeEntryDto
        {
            ConsultantId = entity.ConsultantId,
            TaskId = entity.TaskId,
            DateWorked = entity.DateWorked,
            HoursWorked = entity.HoursWorked
        };

        return await _repository.CreateAsync(dto, cancellationToken);
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
