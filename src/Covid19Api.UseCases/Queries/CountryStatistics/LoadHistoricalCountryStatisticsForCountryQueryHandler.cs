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
        private readonly ICountryStatisticsReadRepository countryStatisticsReadRepository;

        public LoadHistoricalCountryStatisticsForCountryQueryHandler(IMapper mapper,
            ICountryStatisticsReadRepository countryStatisticsReadRepository)
        {
            this.mapper = mapper;
            this.countryStatisticsReadRepository = countryStatisticsReadRepository;
        }

        public async Task<IEnumerable<CountryStatisticDto>> Handle(
            LoadHistoricalCountryStatisticsForCountryQuery request,
            CancellationToken cancellationToken)
        {
            var statsForCountry = await this.countryStatisticsReadRepository.HistoricalAsync(request.MinFetchedAt, request.Country);

            return this.mapper.Map<IEnumerable<CountryStatisticDto>>(statsForCountry);
        }
    }
}