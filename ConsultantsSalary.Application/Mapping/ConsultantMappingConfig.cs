using ConsultantsSalary.Application.Features.Consultants.Commands;
using ConsultantsSalary.Application.Features.Consultants.Queries;
using ConsultantsSalary.Domain.Entities;
using Mapster;

namespace ConsultantsSalary.Application.Mapping;

public static class ConsultantMappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Consultant, ConsultantDto>
            .NewConfig()
            .Map(dest => dest.RoleId, src => src.RoleId)
            .Map(dest => dest.RoleName, src => src.Role.Name);

        TypeAdapterConfig<ConsultantDto, Consultant>
            .NewConfig()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.ProfileImage)
            .Ignore(dest => dest.TaskAssignments)
            .Ignore(dest => dest.TimeEntries)
            .Ignore(dest => dest.Role);

        TypeAdapterConfig<CreateConsultantCommand, Consultant>
            .NewConfig()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.ProfileImage)
            .Ignore(dest => dest.TaskAssignments)
            .Ignore(dest => dest.TimeEntries)
            .Ignore(dest => dest.Role);

        TypeAdapterConfig<UpdateConsultantCommand, Consultant>
            .NewConfig()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.ProfileImage)
            .Ignore(dest => dest.TaskAssignments)
            .Ignore(dest => dest.TimeEntries)
            .Ignore(dest => dest.Role);
    }
}
