using System;
using System.Globalization;
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
    public class AggregateGlobalStatisticsCommandHandler : IRequestHandler<AggregateGlobalStatisticsCommand>
    {
        private readonly ILogger<AggregateGlobalStatisticsCommandHandler> logger;
        private readonly IGlobalStatisticsRepository globalStatisticsRepository;
        private readonly IGlobalStatisticsAggregatesRepository globalStatisticsAggregatesRepository;


        public AggregateGlobalStatisticsCommandHandler(ILogger<AggregateGlobalStatisticsCommandHandler> logger,
            IGlobalStatisticsRepository globalStatisticsRepository,
            IGlobalStatisticsAggregatesRepository globalStatisticsAggregatesRepository)
        {
            this.logger = logger;
            this.globalStatisticsRepository = globalStatisticsRepository;
            this.globalStatisticsAggregatesRepository = globalStatisticsAggregatesRepository;
        }

        public async Task<Unit> Handle(AggregateGlobalStatisticsCommand request, CancellationToken cancellationToken)
        {
            var start = new DateTime(request.Year, request.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = start.MonthsEnd();

            this.logger.LogInformation("Aggregating {entity} from {from} to {to}",
                nameof(GlobalStatistics),
                start.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                end.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));

            var globalStatisticsInRange = await this.globalStatisticsRepository.FindInRangeAsync(start, end);

            if (globalStatisticsInRange is null) return Unit.Value;

            var aggregate = new GlobalStatisticsAggregate(globalStatisticsInRange.Total,
                globalStatisticsInRange.Recovered, globalStatisticsInRange.Deaths, request.Month, request.Year);

            await this.globalStatisticsAggregatesRepository.StoreAsync(aggregate);

            this.logger.LogInformation("Done aggregating {entity} from {from} to {to}",
                nameof(GlobalStatistics),
                start.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
                end.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));

            return Unit.Value;
        }
    }
}