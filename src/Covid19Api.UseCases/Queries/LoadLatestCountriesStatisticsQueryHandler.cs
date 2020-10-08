using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Services.Abstractions.Caching;
using Covid19Api.Services.Abstractions.Parser;
using Covid19Api.UseCases.Abstractions.Queries;
using Covid19Api.UseCases.Filter;
using MediatR;

namespace Covid19Api.UseCases.Queries
{
    public class
        LoadLatestCountriesStatisticsQueryHandler : IRequestHandler<LoadLatestCountriesStatisticsQuery,
            IEnumerable<CountryStatisticsDto>>
    {
        private readonly IMapper mapper;
        private readonly IHtmlDocumentCache htmlDocumentCache;
        private readonly ICountryStatisticsParser countryStatisticsParser;

        public LoadLatestCountriesStatisticsQueryHandler(IMapper mapper, IHtmlDocumentCache htmlDocumentCache,
            ICountryStatisticsParser countryStatisticsParser)
        {
            this.mapper = mapper;
            this.htmlDocumentCache = htmlDocumentCache;
            this.countryStatisticsParser = countryStatisticsParser;
        }

        public async Task<IEnumerable<CountryStatisticsDto>> Handle(LoadLatestCountriesStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var fetchedAt = DateTime.UtcNow;

            var countries = await this.countryStatisticsParser
                .ParseAsync(fetchedAt, CountryStatsFilter.ValidOnly.Value);

            return this.mapper.Map<IEnumerable<CountryStatisticsDto>>(countries)
                .OrderByDescending(country => country!.TotalCases);
        }
    }
}