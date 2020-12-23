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

                        nextRun = this.CalculateNextRun();
                    }
                }
                catch (Exception e)
                {
                    this.logger.LogCritical(e, e.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }

        private DateTime CalculateInitialExecutionTime()
        {
            var currentTime = DateTime.UtcNow;
            var minutesDiff = 60 - currentTime.Minute;
            var nextExecution = currentTime.AddMinutes(minutesDiff).AddSeconds(-currentTime.Second);
            this.logger.LogInformation("Next refresh run {nextRun}", nextExecution.ToString("O"));
            return nextExecution;
        }

        private DateTime CalculateNextRun()
        {
            var nextExecution = DateTime.UtcNow.AddHours(4);
            if (nextExecution.Day != DateTime.UtcNow.Day)
            {
                nextExecution = DateTime.UtcNow.Date.AddDays(1);
            }

            this.logger.LogInformation("Next refresh run {nextRun}", nextExecution.ToString("O"));
            return nextExecution;
        }

        private async Task ProcessAsync()
        {
            this.logger.LogInformation("Start fetching html document");

            using var scope = this.serviceProvider.CreateScope();

            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var fetchedAt = DateTime.UtcNow;

            this.logger.LogInformation("Refreshing global-statistics");

            var refreshGlobalStatisticsCommand = new RefreshGlobalStatisticsCommand(fetchedAt);

            await mediator.Send(refreshGlobalStatisticsCommand);

            this.logger.LogInformation("Refreshing countries-statistics");

            var refreshCountriesStatisticsCommand = new RefreshCountriesStatisticsCommand(fetchedAt);

            await mediator.Send(refreshCountriesStatisticsCommand);
        }
    }
}