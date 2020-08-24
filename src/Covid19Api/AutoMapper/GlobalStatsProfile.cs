using AutoMapper;
using Covid19Api.Domain;
using Covid19Api.Presentation.Response;

// ReSharper disable UnusedType.Global

// ReSharper disable UnusedMember.Global
namespace Covid19Api.AutoMapper
{
    public class GlobalStatsProfile : Profile
    {
        public GlobalStatsProfile()
        {
            this.CreateMap<GlobalStatistics, GlobalStatisticsDto>()
                .ConvertUsing(src =>
                    new GlobalStatisticsDto(src.Total, src.Recovered, src.Deaths, src.FetchedAt));
        }
    }
}