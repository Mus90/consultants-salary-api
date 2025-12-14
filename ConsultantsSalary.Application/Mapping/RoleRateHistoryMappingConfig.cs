using ConsultantsSalary.Application.Dtos;
using ConsultantsSalary.Domain.Entities;
using Mapster;

namespace ConsultantsSalary.Application.Mapping;

public static class RoleRateHistoryMappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<RoleRateHistory, RoleRateHistoryDto>
            .NewConfig()
            .Map(dest => dest.RoleName, src => src.Role.Name);

        TypeAdapterConfig<CreateRoleRateHistoryDto, RoleRateHistory>
            .NewConfig()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.EndDate)
            .Ignore(dest => dest.Role);

        TypeAdapterConfig<RoleRateHistoryDto, RoleRateHistory>
            .NewConfig()
            .Ignore(dest => dest.Id)
            .Ignore(dest => dest.RoleId)
            .Ignore(dest => dest.Role);
    }
}
