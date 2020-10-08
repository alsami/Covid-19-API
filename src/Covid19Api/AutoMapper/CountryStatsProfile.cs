using AutoMapper;
using Covid19Api.Domain;
using Covid19Api.Presentation.Response;

// ReSharper disable UnusedMember.Global

namespace Covid19Api.AutoMapper
{
    public class CountryStatsProfile : Profile
    {
        public CountryStatsProfile()
        {
            this.CreateMap<CountryStatistics, CountryStatisticsDto>()
                .ConvertUsing(src => new CountryStatisticsDto(src.Country, src.CountryCode ?? "N/A", src.TotalCases,
                    src.NewCases, src.TotalDeaths,
                    src.NewDeaths, src.RecoveredCases, src.ActiveCases, src.SeriousCases, src.FetchedAt));
        }
    }
}