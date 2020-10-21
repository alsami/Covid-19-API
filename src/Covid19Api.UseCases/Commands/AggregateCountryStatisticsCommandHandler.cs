using System;
using System.Threading;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Commands;
using Covid19Api.UseCases.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Covid19Api.UseCases.Commands
{
    public class AggregateCountryStatisticsCommandHandler : IRequestHandler<AggregateCountryStatisticsCommand>
    {
        private readonly ILogger<AggregateGlobalStatisticsCommandHandler> logger;
        private readonly ICountryStatisticsRepository countryStatisticsRepository;
        private readonly ICountryStatisticsAggregatesRepository countryStatisticsAggregatesRepository;

        public AggregateCountryStatisticsCommandHandler(ILogger<AggregateGlobalStatisticsCommandHandler> logger, ICountryStatisticsRepository countryStatisticsRepository, ICountryStatisticsAggregatesRepository countryStatisticsAggregatesRepository)
        {
            this.logger = logger;
            this.countryStatisticsRepository = countryStatisticsRepository;
            this.countryStatisticsAggregatesRepository = countryStatisticsAggregatesRepository;
        }

        public async Task<Unit> Handle(AggregateCountryStatisticsCommand request, CancellationToken cancellationToken)
        {
            this.logger.LogInformation("Aggregating statistics for countries");
            this.logger.LogDebug("Countries that are being aggregated: {countries}",
                string.Join(", ", request.Countries));
            var start = new DateTime(request.Year, request.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = start.MonthsEnd();

            foreach (var country in request.Countries)
            {
                await AggregateCountryAsync(country, start, end);
                await Task.Delay(TimeSpan.FromMilliseconds(50), cancellationToken);
            }
            
            this.logger.LogInformation("Aggregated statistics for countries");
            
            return Unit.Value;
        }

        private async Task AggregateCountryAsync(string country, DateTime start, DateTime end)
        {
            var statisticInRange = await this.countryStatisticsRepository.FindInRangeAsync(country, start, end);

            if (statisticInRange is null) return;
            
            var aggregate = new CountryStatisticsAggregate(country, statisticInRange.CountryCode, 
                statisticInRange.TotalCases,
                statisticInRange.NewCases,
                statisticInRange.TotalDeaths,
                statisticInRange.NewDeaths,
                statisticInRange.RecoveredCases,
                statisticInRange.ActiveCases,
                start.Month,
                start.Year
                );

            await this.countryStatisticsAggregatesRepository.StoreAsync(aggregate);
        }
    }
}