using ConsultantsSalary.Application.Dtos;
using MediatR;

namespace ConsultantsSalary.Application.Features.Tasks.Commands;

public record CreateTaskCommand(CreateTaskDto Task) : IRequest<TaskDto>;

public record UpdateTaskCommand(Guid Id, UpdateTaskDto Task) : IRequest<TaskDto?>;

public record DeleteTaskCommand(Guid Id) : IRequest<bool>;

public record AssignConsultantToTaskCommand(Guid TaskId, List<Guid> ConsultantIds, DateTime AssignedDate) : IRequest<Unit>;