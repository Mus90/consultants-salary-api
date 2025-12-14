using ConsultantsSalary.Application.Interfaces;
using MediatR;

namespace ConsultantsSalary.Application.Features.Consultants.Commands;

public record UploadConsultantPhotoCommand(Guid Id, byte[] PhotoBytes) : IRequest;

public class UploadConsultantPhotoHandler : IRequestHandler<UploadConsultantPhotoCommand>
{
    private readonly IConsultantRepository _repository;

    public UploadConsultantPhotoHandler(IConsultantRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UploadConsultantPhotoCommand request, CancellationToken cancellationToken)
    {
        await _repository.UpdateProfileImageAsync(request.Id, request.PhotoBytes, cancellationToken);
    }
}

