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
            IEnumerable<CountryStatsDto>>
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

        public async Task<IEnumerable<CountryStatsDto>> Handle(LoadLatestCountriesStatisticsQuery request,
            CancellationToken cancellationToken)
        {
            var fetchedAt = DateTime.UtcNow;

            var document = await this.htmlDocumentCache.LoadAsync();

            var countries = this.countryStatisticsParser
                .Parse(document, fetchedAt)
                .Where(CountryStatsFilter.ValidOnly.Value);

            return this.mapper.Map<IEnumerable<CountryStatsDto>>(countries)
                .OrderByDescending(country => country!.TotalCases);
        }
    }
}