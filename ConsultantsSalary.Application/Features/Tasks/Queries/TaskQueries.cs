using ConsultantsSalary.Application.Dtos;
using MediatR;

namespace ConsultantsSalary.Application.Features.Tasks.Queries;

public record GetAllTasksQuery : IRequest<List<TaskDto>>;

public record GetTaskByIdQuery(Guid Id) : IRequest<TaskDto?>;
