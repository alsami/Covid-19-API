using AutoMapper;
using Covid19Api.Domain;
using Covid19Api.Presentation.Response;

namespace Covid19Api.AutoMapper
{
    public class CountryStatisticsAggregateProfile : Profile
    {
        public CountryStatisticsAggregateProfile()
        {
            this.CreateMap<CountryStatisticsAggregate, CountryStatisticAggregateDto>()
                .ConvertUsing(source => new CountryStatisticAggregateDto(source.Id, source.Country, source.CountryCode,
                    source.Total, source.New, source.Deaths, source.NewDeaths, source.Recovered, source.Active,
                    source.Month,
                    source.Year));
        }
    }
}