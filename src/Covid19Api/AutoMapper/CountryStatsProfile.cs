using AutoMapper;
using Covid19Api.Controllers.Presentation;
using Covid19Api.Domain;

// ReSharper disable UnusedMember.Global

namespace Covid19Api.AutoMapper
{
    public class CountryStatsProfile : Profile
    {
        public CountryStatsProfile()
        {
            this.CreateMap<CountryStats, CountryStatsDto>()
                .ConvertUsing(src => new CountryStatsDto(src.Country, src.TotalCases, src.NewCases, src.TotalDeaths,
                    src.NewDeaths, src.RecoveredCases, src.ActiveCases, src.SeriousCases, src.FetchedAt));
        }
    }
}