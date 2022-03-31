using Covid19Api.UseCases.Abstractions.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Covid19Api.Worker;

public class VaccinationStatisticsRefreshWorker : AbstractHourlyDataRefreshWorker
{
    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public VaccinationStatisticsRefreshWorker(IServiceProvider serviceProvider, ILogger<VaccinationStatisticsRefreshWorker> logger) : base(serviceProvider, logger)
    {
    }

    protected override IRequest CreateCommand() => new RefreshVaccinationStatisticsCommand();

    protected override string RefreshType => "vaccination-statistics";
}