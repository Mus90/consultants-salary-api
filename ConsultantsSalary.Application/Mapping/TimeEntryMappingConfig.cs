using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Domain.Entities;
using Mapster;

namespace ConsultantsSalary.Application.Mapping;

public static class TimeEntryMappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<TimeEntry, TimeEntryDto>
            .NewConfig()
            .Map(dest => dest.ConsultantName, src => $"{src.Consultant.FirstName} {src.Consultant.LastName}")
            .Map(dest => dest.TaskName, src => src.Task.Name)
            .Map(dest => dest.RatePerHour, src => src.RateSnapshot.RatePerHour);
    }
}
