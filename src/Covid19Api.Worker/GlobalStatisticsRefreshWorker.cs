using Covid19Api.UseCases.Abstractions.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Covid19Api.Worker;

public class GlobalStatisticsRefreshWorker : AbstractHourlyDataRefreshWorker
{
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public GlobalStatisticsRefreshWorker(IServiceProvider serviceProvider, ILogger<GlobalStatisticsRefreshWorker> logger) : base(serviceProvider, logger)
    {
    }

    protected override IRequest CreateCommand()
    {
        var currentTime = DateTime.UtcNow;
        var fetchedAt = currentTime.AddSeconds(-currentTime.Second).AddMilliseconds(-currentTime.Millisecond);

        return new RefreshGlobalStatisticsCommand(fetchedAt);
    }

    protected override string RefreshType => "global-statistics";
}