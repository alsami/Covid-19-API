using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Covid19Api.Worker;

public abstract class AbstractHourlyDataRefreshWorker : BackgroundService
{
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger logger;

    protected AbstractHourlyDataRefreshWorker(IServiceProvider serviceProvider, ILogger logger)
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
                this.logger.LogCritical(e, "Error while refreshing {RefreshType}", this.RefreshType);
            }

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
        }
    }

    protected abstract IRequest CreateCommand();
        
    protected abstract string RefreshType { get; }

    private DateTime CalculateInitialExecutionTime()
    {
        var currentTime = DateTime.UtcNow;
        var minutesDiff = 60 - currentTime.Minute;
        var nextExecution = currentTime.AddMinutes(minutesDiff).AddSeconds(-currentTime.Second).AddMilliseconds(-currentTime.Millisecond);
        this.logger.LogInformation("Next refresh run for {RefreshType} at {NextRun}", this.RefreshType, nextExecution.ToString("O"));
        return nextExecution;
    }

    private async Task ProcessAsync()
    {
        using var scope = this.serviceProvider.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(CreateCommand());
    }
}