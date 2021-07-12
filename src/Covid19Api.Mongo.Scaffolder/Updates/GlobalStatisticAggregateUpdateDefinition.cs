using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Mongo.Scaffolder.Abstractions;
using Covid19Api.Mongo.Scaffolder.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Covid19Api.Mongo.Scaffolder.Updates
{
    // ReSharper disable once UnusedType.Global
    public class GlobalStatisticAggregateUpdateDefinition : DatabaseUpdateDefinition
    {
        private readonly Covid19ApiDbContext databaseContext;

        // ReSharper disable once SuggestBaseTypeForParameter
        public GlobalStatisticAggregateUpdateDefinition(ILogger<GlobalStatisticAggregateUpdateDefinition> logger,
            Covid19ApiDbContext databaseContext) : base(logger)
        {
            this.databaseContext = databaseContext;
        }

        public override int Version => 2;

        protected override async Task ExecuteAsync()
        {
            await this.databaseContext.Database.CreateCollectionIfNotExistsAsync(CollectionNames.GlobalStatisticAggregates);

            var monthIndex = Builders<GlobalStatisticsAggregate>
                .IndexKeys
                .Descending(statistics => statistics.Month);

            var monthIndexModel = new CreateIndexModel<GlobalStatisticsAggregate>(monthIndex, new CreateIndexOptions
            {
                Name = $"{nameof(GlobalStatisticsAggregate)}_month_descending"
            });

            var yearIndex = Builders<GlobalStatisticsAggregate>
                .IndexKeys
                .Descending(statistics => statistics.Year);

            var yearIndexModel = new CreateIndexModel<GlobalStatisticsAggregate>(yearIndex, new CreateIndexOptions
            {
                Name = $"{nameof(GlobalStatisticsAggregate)}_year_descending"
            });

            var yearMonthIndex = Builders<GlobalStatisticsAggregate>
                .IndexKeys
                .Combine(yearIndex, monthIndex);

            var yearMonthIndexModel = new CreateIndexModel<GlobalStatisticsAggregate>(yearMonthIndex,
                new CreateIndexOptions
                {
                    Name = $"{nameof(GlobalStatisticsAggregate)}_year_month",
                });

            var collection =
                this.databaseContext.Database.GetCollection<GlobalStatisticsAggregate>(CollectionNames.GlobalStatisticAggregates);

            await collection.Indexes.CreateManyAsync(new[]
            {
                monthIndexModel,
                yearIndexModel,
                yearMonthIndexModel
            });
        }
    }
}