using System;
using System.Threading;
using System.Threading.Tasks;
using Covid19Api.UseCases.Abstractions.Commands;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Covid19Api.Worker
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DataRefreshWorker : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<DataRefreshWorker> logger;

        public DataRefreshWorker(IServiceProvider serviceProvider,
            ILogger<DataRefreshWorker> logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var nextRun = this.CalculateInitialExecutionTime();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (DateTime.UtcNow >= nextRun)
                    {
                        await this.ProcessAsync();

                        nextRun = this.CalculateInitialExecutionTime();
                    }
                }
                catch (Exception e)
                {
                    this.logger.LogCritical(e, "Error while refreshing data!");
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private DateTime CalculateInitialExecutionTime()
        {
            var currentTime = DateTime.UtcNow;
            var minutesDiff = 60 - currentTime.Minute;
            var nextExecution = currentTime.AddMinutes(minutesDiff).AddSeconds(-currentTime.Second).AddMilliseconds(-currentTime.Millisecond);
            this.logger.LogInformation("Next refresh run {NextRun}", nextExecution.ToString("O"));
            return nextExecution;
        }

        private async Task ProcessAsync()
        {
            using var scope = this.serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var currentTime = DateTime.UtcNow;
            var fetchedAt = currentTime.AddSeconds(-currentTime.Second).AddMilliseconds(-currentTime.Millisecond);

            await Task.WhenAll(
                this.RefreshGlobalStatistics(mediator, fetchedAt),
                this.RefreshCountryStatistics(mediator, fetchedAt));
        }

        private async Task RefreshCountryStatistics(ISender mediator, DateTime fetchedAt)
        {
            this.logger.LogInformation("Refreshing countries-statistics");

            var refreshCountriesStatisticsCommand = new RefreshCountriesStatisticsCommand(fetchedAt);

            await mediator.Send(refreshCountriesStatisticsCommand);
        }

        private async Task RefreshGlobalStatistics(ISender mediator, DateTime fetchedAt)
        {
            this.logger.LogInformation("Refreshing global-statistics");

            var refreshGlobalStatisticsCommand = new RefreshGlobalStatisticsCommand(fetchedAt);

            await mediator.Send(refreshGlobalStatisticsCommand);
        }
    }
}