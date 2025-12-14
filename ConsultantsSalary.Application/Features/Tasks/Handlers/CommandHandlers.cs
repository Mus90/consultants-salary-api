using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Application.Features.Tasks.Commands;
using ConsultantsSalary.Application.Interfaces;
using MediatR;

namespace ConsultantsSalary.Application.Features.Tasks.Handlers;

public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _repository;

    public CreateTaskHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        return await _repository.CreateAsync(request.Task, cancellationToken);
    }
}

public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, TaskDto?>
{
    private readonly ITaskRepository _repository;

    public UpdateTaskHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<TaskDto?> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.TaskExistsAsync(request.Id, cancellationToken))
            throw new InvalidOperationException("Task not found");

        return await _repository.UpdateAsync(request.Id, request.Task, cancellationToken);
    }
}

public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, bool>
{
    private readonly ITaskRepository _repository;

    public DeleteTaskHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.Id, cancellationToken);
    }
}

public class AssignMultipleConsultantsToTaskHandler : IRequestHandler<AssignConsultantToTaskCommand, Unit>
{
    private readonly ITaskRepository _repository;

    public AssignMultipleConsultantsToTaskHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(AssignConsultantToTaskCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.TaskExistsAsync(request.TaskId, cancellationToken))
            throw new InvalidOperationException("Task not found");

        foreach (var consultantId in request.ConsultantIds)
        {
            if (!await _repository.ConsultantExistsAsync(consultantId, cancellationToken))
                throw new InvalidOperationException($"Consultant {consultantId} not found");
        }

        await _repository.AssignMultipleConsultantsAsync(request.TaskId, request.ConsultantIds, cancellationToken);
        return Unit.Value;
    }
}