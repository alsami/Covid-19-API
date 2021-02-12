using AutoMapper;
using Covid19Api.Domain;
using Covid19Api.Presentation.Response;

// ReSharper disable UnusedMember.Global

namespace Covid19Api.AutoMapper
{
    public class CountryStatisticsProfile : Profile
    {
        public CountryStatisticsProfile()
        {
            this.CreateMap<CountryStatistic, CountryStatisticDto>()
                .ConvertUsing(src => new CountryStatisticDto(src.Country, src.CountryCode, src.TotalCases,
                    src.NewCases, src.TotalDeaths, src.NewDeaths, src.RecoveredCases, src.ActiveCases, src.FetchedAt));
        }
    }
}