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
    public class GlobalStatisticsAggregationWorker : BackgroundService
    {
        private readonly ILogger<GlobalStatisticsAggregationWorker> logger;
        private readonly IServiceProvider serviceProvider;

        public GlobalStatisticsAggregationWorker(ILogger<GlobalStatisticsAggregationWorker> logger,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // UTC day switch
            var nextRun = DateTime.UtcNow.Date.AddDays(1);
            this.logger.LogInformation("Next global-statistics aggregation at {At}", nextRun.ToString("dd.MM.yyyy HH:mm:ss"));
            while (!stoppingToken.IsCancellationRequested)
            {
                if (nextRun <= DateTime.UtcNow)
                {
                    await ExecuteAggregationAsync(nextRun, stoppingToken);
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
                var command = new AggregateGlobalStatisticsCommand(nextRun.Month, nextRun.Year);
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                await mediator.Send(command, stoppingToken);
            }
            catch (Exception e)
            {
                this.logger.LogCritical(e, "Error while aggregating global-statistics");
            }
        }
    }
}