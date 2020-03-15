using AutoMapper;
using Covid19Api.Controllers.Presentation;
using Covid19Api.Domain;

namespace Covid19Api.AutoMapper
{
    // ReSharper disable once UnusedType.Global
    public class ClosedCasesStatsProfile : Profile
    {
        public ClosedCasesStatsProfile()
        {
            this.CreateMap<ClosedCaseStats, ClosedCaseStatsDto>()
                .ConvertUsing(src => new ClosedCaseStatsDto(src.Total, src.Recovered, src.Deaths, src.FetchedAt));
        }
    }
}