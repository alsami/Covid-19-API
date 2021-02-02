using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.Services.Abstractions.Calculators;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatistics;
using MediatR;

namespace Covid19Api.UseCases.Queries.CountryStatistics
{
    public class CalculateVaryForCountriesStatisticsQueryHandler : IRequestHandler<CalculateVaryForCountriesStatisticsQuery, IEnumerable<CountryVaryStatisticContainerDto>>
    {
        private readonly ICountryStatisticsReadRepository countryStatisticsReadRepository;
        private readonly ICountryVaryStatisticsCalculator countryVaryStatisticsCalculator;

        public CalculateVaryForCountriesStatisticsQueryHandler(ICountryStatisticsReadRepository countryStatisticsReadRepository, ICountryVaryStatisticsCalculator countryVaryStatisticsCalculator)
        {
            this.countryStatisticsReadRepository = countryStatisticsReadRepository;
            this.countryVaryStatisticsCalculator = countryVaryStatisticsCalculator;
        }

        public async Task<IEnumerable<CountryVaryStatisticContainerDto>> Handle(CalculateVaryForCountriesStatisticsQuery request, CancellationToken cancellationToken)
        {
            var historicalCountriesStatistics = await this.countryStatisticsReadRepository.HistoricalAsync(request.MinFetchedAt);

            return countryVaryStatisticsCalculator.Calculate(historicalCountriesStatistics).OrderByDescending(container => container.Time);
        }
    }
}