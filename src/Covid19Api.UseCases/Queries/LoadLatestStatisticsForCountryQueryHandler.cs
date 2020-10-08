using System;
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
        LoadLatestStatisticsForCountryQueryHandler : IRequestHandler<LoadLatestStatisticsForCountryQuery,
            CountryStatisticsDto>
    {
        private readonly IMapper mapper;
        private readonly IHtmlDocumentCache htmlDocumentCache;
        private readonly ICountryStatisticsParser countryStatisticsParser;

        public LoadLatestStatisticsForCountryQueryHandler(IMapper mapper, IHtmlDocumentCache htmlDocumentCache,
            ICountryStatisticsParser countryStatisticsParser)
        {
            this.mapper = mapper;
            this.htmlDocumentCache = htmlDocumentCache;
            this.countryStatisticsParser = countryStatisticsParser;
        }

        public async Task<CountryStatisticsDto> Handle(LoadLatestStatisticsForCountryQuery request,
            CancellationToken cancellationToken)
        {
            var fetchedAt = DateTime.UtcNow;

            var countries =
                await this.countryStatisticsParser.ParseAsync(fetchedAt, CountryStatsFilter.ValidOnly.Value);

            var wanted = countries
                .SingleOrDefault(stats =>
                    string.Equals(stats!.Country, request.Country, StringComparison.InvariantCultureIgnoreCase));

            return this.mapper.Map<CountryStatisticsDto>(wanted);
        }
    }
}