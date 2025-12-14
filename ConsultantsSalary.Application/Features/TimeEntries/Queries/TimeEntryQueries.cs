using ConsultantsSalary.Application.Dtos;
using MediatR;

namespace ConsultantsSalary.Application.Features.TimeEntries.Queries;

public record GetAllTimeEntriesQuery : IRequest<List<TimeEntryDto>>;

public record GetTimeEntriesByConsultantIdQuery(Guid ConsultantId) : IRequest<List<TimeEntryDto>>;

public record GetTimeEntriesByTaskIdQuery(Guid TaskId) : IRequest<List<TimeEntryDto>>;

public record GetTimeEntryByIdQuery(Guid Id) : IRequest<TimeEntryDto?>;
