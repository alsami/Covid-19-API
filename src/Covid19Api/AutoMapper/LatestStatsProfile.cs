using AutoMapper;
using Covid19Api.Controllers.Presentation;
using Covid19Api.Domain;

namespace Covid19Api.AutoMapper
{
    // ReSharper disable once UnusedType.Global
    public class LatestStatsProfile : Profile
    {
        public LatestStatsProfile()
        {
            this.CreateMap<LatestStats, LatestStatsDto>()
                .ConvertUsing(src => new LatestStatsDto(src.Total, src.Recovered, src.Deaths, src.FetchedAt));
        }
    }
}