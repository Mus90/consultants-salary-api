using ConsultantsSalary.Application.Dtos;
using MediatR;

namespace ConsultantsSalary.Application.Features.TimeEntries.Commands;

public record CreateTimeEntryCommand(CreateTimeEntryDto TimeEntry) : IRequest<TimeEntryDto>;

public record DeleteTimeEntryCommand(Guid Id) : IRequest<bool>;
