using AutoMapper;
using Covid19Api.Domain;
using Covid19Api.Presentation.Response;

// ReSharper disable UnusedType.Global

// ReSharper disable UnusedMember.Global
namespace Covid19Api.AutoMapper;

public class GlobalStatisticsProfile : Profile
{
    public GlobalStatisticsProfile()
    {
        this.CreateMap<GlobalStatistics, GlobalStatisticDto>()
            .ConvertUsing(src =>
                new GlobalStatisticDto(src.Total, src.Recovered, src.Deaths, src.FetchedAt));
    }
}