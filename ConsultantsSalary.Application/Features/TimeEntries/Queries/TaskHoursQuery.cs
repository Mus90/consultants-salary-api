using ConsultantsSalary.Application.Dtos;
using MediatR;

namespace ConsultantsSalary.Application.Features.TimeEntries.Queries;

public record GetTaskHoursQuery(Guid TaskId, Guid ConsultantId) : IRequest<TaskHoursDto>;
