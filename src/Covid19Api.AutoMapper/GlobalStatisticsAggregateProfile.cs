using AutoMapper;
using Covid19Api.Domain;
using Covid19Api.Presentation.Response;

namespace Covid19Api.AutoMapper
{
    public class GlobalStatisticsAggregateProfile : Profile
    {
        public GlobalStatisticsAggregateProfile()
        {
            this.CreateMap<GlobalStatisticsAggregate, GlobalStatisticAggregateDto>()
                .ConstructUsing(source => new GlobalStatisticAggregateDto(source.Id, source.Total, source.Recovered,
                    source.Deaths, source.Month, source.Year));
        }
    }
}