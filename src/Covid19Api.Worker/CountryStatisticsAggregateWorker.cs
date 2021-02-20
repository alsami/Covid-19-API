using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Covid19Api.UseCases.Abstractions.Commands;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatistics;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Covid19Api.Worker
{
    public class CountryStatisticsAggregateWorker : BackgroundService
    {
        private readonly ILogger<CountryStatisticsAggregateWorker> logger;
        private readonly IServiceProvider serviceProvider;

        public CountryStatisticsAggregateWorker(ILogger<CountryStatisticsAggregateWorker> logger,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // UTC day switch
            var nextRun = DateTime.UtcNow.Date.AddDays(1);
            this.logger.LogInformation("Next country-statistics aggregation at {At}", nextRun.ToString("dd.MM.yyyy HH:mm:ss"));
            while (!stoppingToken.IsCancellationRequested)
            {
                if (nextRun <= DateTime.UtcNow)
                {
                    await this.ExecuteAggregationAsync(nextRun, stoppingToken);
                    nextRun = nextRun.AddDays(1);
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private async Task ExecuteAggregationAsync(DateTime nextRun, CancellationToken stoppingToken)
        {
            try
            {
                using var scope = this.serviceProvider.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var query = new LoadCurrentStatisticsForCountyQuery();
                var countries = (await mediator.Send(query, stoppingToken)).Select(country => country.Country);
                var command = new AggregateCountryStatisticsCommand(countries.ToArray(), nextRun.Month, nextRun.Year);
                await mediator.Send(command, stoppingToken);
            }
            catch (Exception e)
            {
                this.logger.LogCritical(e, "Error while aggregating country-statistics");
            }
        }
    }
}