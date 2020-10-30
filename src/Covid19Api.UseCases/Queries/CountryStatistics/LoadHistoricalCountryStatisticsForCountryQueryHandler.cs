using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatistics;
using MediatR;

namespace Covid19Api.UseCases.Queries.CountryStatistics
{
    public class
        LoadHistoricalCountryStatisticsForCountryQueryHandler : IRequestHandler<
            LoadHistoricalCountryStatisticsForCountryQuery,
            IEnumerable<CountryStatisticDto>>
    {
        private readonly IMapper mapper;
        private readonly ICountryStatisticsRepository countryStatisticsRepository;

        public LoadHistoricalCountryStatisticsForCountryQueryHandler(IMapper mapper,
            ICountryStatisticsRepository countryStatisticsRepository)
        {
            this.mapper = mapper;
            this.countryStatisticsRepository = countryStatisticsRepository;
        }

        public async Task<IEnumerable<CountryStatisticDto>> Handle(
            LoadHistoricalCountryStatisticsForCountryQuery request,
            CancellationToken cancellationToken)
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-9);

            var statsForCountry = await this.countryStatisticsRepository.HistoricalAsync(minFetchedAt, request.Country);

            return this.mapper.Map<IEnumerable<CountryStatisticDto>>(statsForCountry);
        }
    }
}