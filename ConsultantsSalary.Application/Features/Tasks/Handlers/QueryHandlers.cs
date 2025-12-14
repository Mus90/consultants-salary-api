using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Features.Tasks.Queries;
using ConsultantsSalary.Application.Interfaces;
using MediatR;
using Task = System.Threading.Tasks.Task;

namespace ConsultantsSalary.Application.Features.Tasks.Handlers;

public class GetAllTasksHandler : IRequestHandler<GetAllTasksQuery, List<TaskDto>>
{
    private readonly ITaskRepository _repository;

    public GetAllTasksHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TaskDto>> Handle(GetAllTasksQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}

public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, TaskDto?>
{
    private readonly ITaskRepository _repository;

    public GetTaskByIdHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<TaskDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id, cancellationToken);
    }
}
