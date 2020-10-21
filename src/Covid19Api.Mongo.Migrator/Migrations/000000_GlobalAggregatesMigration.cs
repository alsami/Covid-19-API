using System;
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
    public class GlobalAggregatesMigration : DatabaseMigration
    {
        private readonly GlobalAggregatesStartConfiguration options;
        private readonly IMediator mediator;

        // ReSharper disable once SuggestBaseTypeForParameter
        public GlobalAggregatesMigration(ILogger<GlobalAggregatesMigration> logger,
            IOptions<GlobalAggregatesStartConfiguration> options, IMediator mediator) : base(logger)
        {
            this.mediator = mediator;
            this.options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }
        
        public override int Number => 0;
        protected override string Name => nameof(GlobalAggregatesMigration);

        protected override async Task ExecuteAsync()
        {
            var next = new DateTime(this.options.Year, this.options.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = DateTime.UtcNow.Date;

            while (true)
            {
                if (next.Month > end.Month && next.Year >= end.Year)
                    break;
                
                var query = new LoadGlobalStatisticsAggregate(next.Month, next.Year);
                var aggregate = await this.mediator.Send(query);
                
                if (aggregate is {})
                {
                    next = next.AddMonths(1);
                    continue;
                }
                
                var command = new AggregateGlobalStatisticsCommand(next.Month, next.Year);
                await this.mediator.Send(command);

                next = next.AddMonths(1);
            }
        }
    }
}