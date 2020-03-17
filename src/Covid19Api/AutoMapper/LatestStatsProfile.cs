using AutoMapper;
using Covid19Api.Controllers.Presentation;
using Covid19Api.Domain;

// ReSharper disable UnusedMember.Global
namespace Covid19Api.AutoMapper
{
    public class LatestStatsProfile : Profile
    {
        public LatestStatsProfile()
        {
            this.CreateMap<GlobalStats, LatestStatsDto>()
                .ConvertUsing(src => new LatestStatsDto(src.Total, src.Recovered, src.Deaths, src.FetchedAt));
        }
    }
}