using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatistics;
using Covid19Api.UseCases.Filter;
using MediatR;

namespace Covid19Api.UseCases.Queries.CountryStatistics
{
    public class
        LoadLatestCountriesStatisticsQueryHandler : IRequestHandler<LoadLatestCountriesStatisticsQuery,
            IEnumerable<CountryStatisticDto>>
    {
        private readonly IMapper mapper;
        private readonly ICountryStatisticsLoader countryStatisticsLoader;

        public LoadLatestCountriesStatisticsQueryHandler(IMapper mapper,
            ICountryStatisticsLoader countryStatisticsLoader)
        {
            this.mapper = mapper;
            this.countryStatisticsLoader = countryStatisticsLoader;
        }

        public async Task<IEnumerable<CountryStatisticDto>> Handle(LoadLatestCountriesStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var fetchedAt = DateTime.UtcNow;

            var countries = await this.countryStatisticsLoader
                .ParseAsync(fetchedAt, CountryStatsFilter.ValidOnly.Value);

            return this.mapper.Map<IEnumerable<CountryStatisticDto>>(countries)
                .OrderByDescending(country => country!.TotalCases);
        }
    }
}