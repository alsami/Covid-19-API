using System;
using Covid19Api.UseCases.Abstractions.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Covid19Api.Worker
{
    public class CountryStatisticsRefreshWorker : AbstractHourlyDataRefreshWorker
    {
        // ReSharper disable once SuggestBaseTypeForParameterInConstructor
        public CountryStatisticsRefreshWorker(IServiceProvider serviceProvider, ILogger<CountryStatisticsRefreshWorker> logger) : base(serviceProvider, logger)
        {
        }

        protected override IRequest CreateCommand()
        {
            var currentTime = DateTime.UtcNow;
            var fetchedAt = currentTime.AddSeconds(-currentTime.Second).AddMilliseconds(-currentTime.Millisecond);

            return new RefreshCountriesStatisticsCommand(fetchedAt);
        }

        protected override string RefreshType => "country-statistics";
    }
}