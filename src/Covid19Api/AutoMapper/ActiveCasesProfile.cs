using AutoMapper;
using Covid19Api.Domain;
using Covid19Api.Presentation.Response;

// ReSharper disable UnusedMember.Global

namespace Covid19Api.AutoMapper
{
    public class ActiveCasesProfile : Profile
    {
        public ActiveCasesProfile()
        {
            this.CreateMap<ActiveCaseStats, ActiveCaseStatsDto>()
                .ConvertUsing(src => new ActiveCaseStatsDto(src.Total, src.Mild, src.Serious, src.FetchedAt));
        }
    }
}