using System;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Mongo.Migrator.Abstractions;
using Covid19Api.Mongo.Migrator.Configuration;
using Covid19Api.UseCases.Abstractions.Commands;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatistics;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Covid19Api.Mongo.Migrator.Migrations
{
    // ReSharper disable once UnusedType.Global
    public class CountryAggregatesMigration : DatabaseMigration
    {
        private readonly CountryAggregatesStartConfiguration options;
        private readonly IMediator mediator;

        // ReSharper disable once SuggestBaseTypeForParameter
        public CountryAggregatesMigration(ILogger<CountryAggregatesMigration> logger,
            IOptions<CountryAggregatesStartConfiguration> options, IMediator mediator) : base(logger)
        {
            this.mediator = mediator;
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public override int Number => 1;
        protected override string Name => nameof(CountryAggregatesMigration);

        protected override async Task ExecuteAsync()
        {
            var next = new DateTime(2020, this.options.MonthStart, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = DateTime.UtcNow.Date;

            while (true)
            {
                if (next > end)
                    break;

                var loadCountriesStatisticsQuery = new LoadCurrentStatisticsForCountyQuery();
                var countries = (await this.mediator.Send(loadCountriesStatisticsQuery))
                    .Select(country => country.Country).ToList();

                var command = new AggregateCountryStatisticsCommand(countries.ToArray(), next.Month, next.Year);
                await this.mediator.Send(command);

                next = next.AddMonths(1);
            }
        }
    }
}