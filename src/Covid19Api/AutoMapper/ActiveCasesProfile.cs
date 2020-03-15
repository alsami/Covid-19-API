using AutoMapper;
using Covid19Api.Controllers.Presentation;
using Covid19Api.Domain;

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