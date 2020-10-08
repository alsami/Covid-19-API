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
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await this.ProcessAsync();
                }
                catch (Exception e)
                {
                    this.logger.LogCritical(e, e.Message);
                }

                var nextRun = DateTime.UtcNow.AddHours(4);

                this.logger.LogInformation("Next run {nextRun}", nextRun.ToString("O"));

                await Task.Delay(TimeSpan.FromHours(4), stoppingToken);
            }
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