using System;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Mongo.Migrator.Abstractions;
using Covid19Api.Mongo.Migrator.Configuration;
using Covid19Api.UseCases.Abstractions.Commands;
using Covid19Api.UseCases.Abstractions.Queries;
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
            var next = new DateTime(this.options.Year, this.options.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = DateTime.UtcNow.Date;

            while (true)
            {
                if (next.Month > end.Month && next.Year >= end.Year)
                    break;
                
                var loadCountriesStatisticsQuery = new LoadLatestCountriesStatisticsQuery();
                var countries = (await this.mediator.Send(loadCountriesStatisticsQuery))
                    .Select(country => country.Country).ToList();

                foreach (var country in countries.ToList())
                {
                    var query = new LoadCountryStatisticsAggregate(country, next.Month, next.Year);
                    var aggregate = await this.mediator.Send(query);

                    await Task.Delay(50);
                    if (aggregate is null) continue;

                    countries.Remove(country);
                }

                if (!countries.Any())
                {
                    next = next.AddMonths(1);
                    await Task.Delay(100);
                    continue;
                }
                
                var command = new AggregateCountryStatisticsCommand(countries.ToArray(), next.Month, next.Year);
                await this.mediator.Send(command);

                next = next.AddMonths(1);
                await Task.Delay(100);
            }
        }
    }
}