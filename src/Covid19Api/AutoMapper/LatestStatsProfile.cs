using AutoMapper;
using Covid19Api.Domain;
using Covid19Api.Presentation;

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