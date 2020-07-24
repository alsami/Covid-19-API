using AutoMapper;
using Covid19Api.Domain;
using Covid19Api.Presentation.Response;

// ReSharper disable UnusedMember.Global
namespace Covid19Api.AutoMapper
{
    public class ClosedCasesStatsProfile : Profile
    {
        public ClosedCasesStatsProfile()
        {
            this.CreateMap<ClosedCaseStats, ClosedCaseStatsDto>()
                .ConvertUsing(src => new ClosedCaseStatsDto(src.Total, src.Recovered, src.Deaths, src.FetchedAt));
        }
    }
}