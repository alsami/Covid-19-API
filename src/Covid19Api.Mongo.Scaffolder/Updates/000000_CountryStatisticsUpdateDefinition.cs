using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Mongo.Scaffolder.Abstractions;
using Covid19Api.Mongo.Scaffolder.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Covid19Api.Mongo.Scaffolder.Updates
{
    public class CountryStatisticsUpdateDefinition : DatabaseUpdateDefinition
    {
        private readonly Covid19ApiDbContext databaseContext;

        // ReSharper disable once SuggestBaseTypeForParameter
        public CountryStatisticsUpdateDefinition(ILogger<CountryStatisticsUpdateDefinition> logger,
            Covid19ApiDbContext databaseContext) : base(logger)
        {
            this.databaseContext = databaseContext;
        }

        public override int Version => 1;

        protected override async Task ExecuteAsync()
        {
            await this.databaseContext.Database.CreateCollectionIfNotExistsAsync(CollectionNames.CountryStatistics);

            var collection =
                this.databaseContext.Database.GetCollection<CountryStatistics>(CollectionNames.CountryStatistics);

            await collection.Indexes.CreateManyAsync(new[]
            {
                CreateTotalCasesFetchedAtIndexModel(),
                CreateTotalCasesFetchedAtCountryIndexModel()
            });
        }

        private static CreateIndexModel<CountryStatistics> CreateTotalCasesFetchedAtIndexModel()
        {
            var totalCasesFetchedAtIndex = Builders<CountryStatistics>
                .IndexKeys
                .Descending(statistics => statistics.TotalCases)
                .Descending(statistics => statistics.FetchedAt);

            var totalCasesFetchedAtIndexModel = new CreateIndexModel<CountryStatistics>(totalCasesFetchedAtIndex,
                new CreateIndexOptions
                {
                    Name = $"{CollectionNames.CountryStatistics}_totalCases_fetchedAt_descending"
                });
            return totalCasesFetchedAtIndexModel;
        }

        private static CreateIndexModel<CountryStatistics> CreateTotalCasesFetchedAtCountryIndexModel()
        {
            var totalCasesFetchedAtCountryIndex = Builders<CountryStatistics>
                .IndexKeys
                .Descending(statistics => statistics.TotalCases)
                .Descending(statistics => statistics.FetchedAt)
                .Ascending(statistics => statistics.Country);

            var totalCasesFetchedAtCountryIndexModel = new CreateIndexModel<CountryStatistics>(
                totalCasesFetchedAtCountryIndex,
                new CreateIndexOptions
                {
                    Name = $"{CollectionNames.CountryStatistics}_totalCases_fetchedAt_descending_country_ascending"
                });
            return totalCasesFetchedAtCountryIndexModel;
        }
    }
}