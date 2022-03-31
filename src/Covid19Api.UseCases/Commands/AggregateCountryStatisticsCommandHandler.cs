using System.Globalization;
using Covid19Api.Domain;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Commands;
using Covid19Api.UseCases.Extensions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Covid19Api.UseCases.Commands;

public class AggregateCountryStatisticsCommandHandler : IRequestHandler<AggregateCountryStatisticsCommand>
{
    private readonly List<CountryStatisticsAggregate> aggregates = new List<CountryStatisticsAggregate>(300);

    private readonly ILogger<AggregateGlobalStatisticsCommandHandler> logger;
    private readonly ICountryStatisticsReadRepository countryStatisticsReadRepository;
    private readonly ICountryStatisticsAggregatesWriteRepository countryStatisticsAggregatesWriteRepository;

    public AggregateCountryStatisticsCommandHandler(ILogger<AggregateGlobalStatisticsCommandHandler> logger,
        ICountryStatisticsReadRepository countryStatisticsReadRepository,
        ICountryStatisticsAggregatesWriteRepository countryStatisticsAggregatesWriteRepository)
    {
        this.logger = logger;
        this.countryStatisticsReadRepository = countryStatisticsReadRepository;
        this.countryStatisticsAggregatesWriteRepository = countryStatisticsAggregatesWriteRepository;
    }

    public async Task<Unit> Handle(AggregateCountryStatisticsCommand request, CancellationToken cancellationToken)
    {
        var start = new DateTime(request.Year, request.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = start.MonthsEnd();
        var countries = request.Countries.OrderBy(country => country).ToList();

        this.logger.LogInformation("Aggregating {entity} from {from} to {to}",
            nameof(CountryStatistic),
            start.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
            end.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));
        this.logger.LogDebug("Countries that are being aggregated:\n{countries}",
            string.Join(",\n", countries));

        foreach (var country in countries)
        {
            var aggregate = await this.CreateCountryAggregateAsync(country, start, end);

            if (aggregate is {}) this.aggregates.Add(aggregate);
        }

        await this.countryStatisticsAggregatesWriteRepository.StoreManyAsync(aggregates);
        this.aggregates.Clear();

        this.logger.LogInformation("Done aggregating {entity} from {from} to {to}",
            nameof(CountryStatistic),
            start.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture),
            end.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture));

        return Unit.Value;
    }

    private async Task<CountryStatisticsAggregate?> CreateCountryAggregateAsync(string country, DateTime start,
        DateTime end)
    {
        var statisticInRange = await this.countryStatisticsReadRepository.FindInRangeAsync(country, start, end);

        if (statisticInRange is null) return null;

        return new CountryStatisticsAggregate(country, statisticInRange.CountryCode,
            statisticInRange.TotalCases,
            statisticInRange.NewCases,
            statisticInRange.TotalDeaths,
            statisticInRange.NewDeaths,
            statisticInRange.RecoveredCases,
            statisticInRange.ActiveCases,
            start.Month,
            start.Year
        );
    }
}