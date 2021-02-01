using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.Services.Abstractions.Calculators;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatistics;
using MediatR;

namespace Covid19Api.UseCases.Queries.CountryStatistics
{
    public class CalculateVaryForCountryStatisticsQueryHandler : IRequestHandler<CalculateVaryForCountryStatisticsQuery, IEnumerable<CountryVaryStatisticContainerDto>>
    {
        private readonly ICountryStatisticsReadRepository countryStatisticsReadRepository;
        private readonly ICountryVaryStatisticsCalculator countryVaryStatisticsCalculator;

        public CalculateVaryForCountryStatisticsQueryHandler(ICountryStatisticsReadRepository countryStatisticsReadRepository, ICountryVaryStatisticsCalculator countryVaryStatisticsCalculator)
        {
            this.countryStatisticsReadRepository = countryStatisticsReadRepository;
            this.countryVaryStatisticsCalculator = countryVaryStatisticsCalculator;
        }

        public async Task<IEnumerable<CountryVaryStatisticContainerDto>> Handle(CalculateVaryForCountryStatisticsQuery request, CancellationToken cancellationToken)
        {
            var historicalCountriesStatistics = await this.countryStatisticsReadRepository.HistoricalAsync(request.MinFetchedAt, request.Country);

            return this.countryVaryStatisticsCalculator.Calculate(historicalCountriesStatistics);
        }
    }
}