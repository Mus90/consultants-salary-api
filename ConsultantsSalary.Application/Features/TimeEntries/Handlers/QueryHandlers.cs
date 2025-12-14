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

public class GetConsultantTotalHandler : IRequestHandler<GetConsultantTotalQuery, ConsultantTotalDto>
{
    private readonly ITimeEntryRepository _repository;

    public GetConsultantTotalHandler(ITimeEntryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ConsultantTotalDto> Handle(GetConsultantTotalQuery request, CancellationToken cancellationToken)
    {
        var timeEntries = await _repository.GetByConsultantIdAndDateRangeAsync(
            request.ConsultantId, 
            request.StartDate, 
            request.EndDate, 
            cancellationToken);

        if (!timeEntries.Any())
        {
            return new ConsultantTotalDto
            {
                ConsultantId = request.ConsultantId,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                TotalHoursWorked = 0,
                TotalAmountDue = 0,
                TimeEntryCount = 0,
                TimeEntries = new List<TimeEntrySummaryDto>()
            };
        }

        var consultantName = timeEntries.First().ConsultantName;
        var totalHours = timeEntries.Sum(te => te.HoursWorked);
        var totalAmount = timeEntries.Sum(te => te.HoursWorked * te.RatePerHour);

        var timeEntrySummaries = timeEntries.Select(te => new TimeEntrySummaryDto
        {
            DateWorked = te.DateWorked,
            HoursWorked = te.HoursWorked,
            HourlyRate = te.RatePerHour,
            Amount = te.HoursWorked * te.RatePerHour,
            TaskName = te.TaskName
        }).ToList();

        return new ConsultantTotalDto
        {
            ConsultantId = request.ConsultantId,
            ConsultantName = consultantName,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            TotalHoursWorked = totalHours,
            TotalAmountDue = totalAmount,
            TimeEntryCount = timeEntries.Count,
            TimeEntries = timeEntrySummaries
        };
    }
}

public class GetTaskHoursHandler : IRequestHandler<GetTaskHoursQuery, TaskHoursDto>
{
    private readonly ITimeEntryRepository _repository;

    public GetTaskHoursHandler(ITimeEntryRepository repository)
    {
        _repository = repository;
    }

    public async Task<TaskHoursDto> Handle(GetTaskHoursQuery request, CancellationToken cancellationToken)
    {
        var timeEntries = await _repository.GetByTaskAndConsultantAsync(
            request.TaskId,
            request.ConsultantId, 
            cancellationToken);

        if (!timeEntries.Any())
        {
            return new TaskHoursDto
            {
                TaskId = request.TaskId,
                ConsultantId = request.ConsultantId,
                TotalHoursWorked = 0,
                TotalAmountDue = 0,
                TimeEntryCount = 0,
                TimeEntries = new List<TaskTimeEntrySummaryDto>()
            };
        }

        var taskName = timeEntries.First().TaskName;
        var consultantName = timeEntries.First().ConsultantName;
        var totalHours = timeEntries.Sum(te => te.HoursWorked);
        var totalAmount = timeEntries.Sum(te => te.HoursWorked * te.RatePerHour);

        var timeEntrySummaries = timeEntries.Select(te => new TaskTimeEntrySummaryDto
        {
            DateWorked = te.DateWorked,
            HoursWorked = te.HoursWorked,
            HourlyRate = te.RatePerHour,
            Amount = te.HoursWorked * te.RatePerHour
        }).ToList();

        return new TaskHoursDto
        {
            TaskId = request.TaskId,
            TaskName = taskName,
            ConsultantId = request.ConsultantId,
            ConsultantName = consultantName,
            TotalHoursWorked = totalHours,
            TotalAmountDue = totalAmount,
            TimeEntryCount = timeEntries.Count,
            TimeEntries = timeEntrySummaries
        };
    }
}
