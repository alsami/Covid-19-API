using System;
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
        LoadLatestCountryStatisticsForCountryQueryHandler : IRequestHandler<LoadLatestStatisticsForCountryQuery,
            CountryStatisticsDto>
    {
        private readonly IMapper mapper;
        private readonly IHtmlDocumentLoader htmlDocumentLoader;
        private readonly ICountryStatisticsLoader countryStatisticsLoader;

        public LoadLatestCountryStatisticsForCountryQueryHandler(IMapper mapper, IHtmlDocumentLoader htmlDocumentLoader,
            ICountryStatisticsLoader countryStatisticsLoader)
        {
            this.mapper = mapper;
            this.htmlDocumentLoader = htmlDocumentLoader;
            this.countryStatisticsLoader = countryStatisticsLoader;
        }

        public async Task<CountryStatisticsDto> Handle(LoadLatestStatisticsForCountryQuery request,
            CancellationToken cancellationToken)
        {
            var fetchedAt = DateTime.UtcNow;

            var countries =
                await this.countryStatisticsLoader.ParseAsync(fetchedAt, CountryStatsFilter.ValidOnly.Value);

            var wanted = countries
                .SingleOrDefault(stats =>
                    string.Equals(stats!.Country, request.Country, StringComparison.InvariantCultureIgnoreCase));

            return this.mapper.Map<CountryStatisticsDto>(wanted);
        }
    }
}