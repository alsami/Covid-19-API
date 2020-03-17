using AutoMapper;
using Covid19Api.Controllers.Presentation;
using Covid19Api.Domain;
// ReSharper disable UnusedType.Global

// ReSharper disable UnusedMember.Global
namespace Covid19Api.AutoMapper
{
    public class GlobalStatsProfile : Profile
    {
        public GlobalStatsProfile()
        {
            this.CreateMap<GlobalStats, GlobalStatsDto>()
                .ConvertUsing(src => new Controllers.Presentation.GlobalStatsDto(src.Total, src.Recovered, src.Deaths, src.FetchedAt));
        }
    }
}