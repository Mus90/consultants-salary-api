using ConsultantsSalary.Application.Dtos;
using MediatR;

namespace ConsultantsSalary.Application.Features.TimeEntries.Queries;

public record GetConsultantTotalQuery(Guid ConsultantId, DateTime StartDate, DateTime EndDate) : IRequest<ConsultantTotalDto>;
