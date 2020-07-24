using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Mongo.Scaffolder.Abstractions;
using Covid19Api.Mongo.Scaffolder.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Covid19Api.Mongo.Scaffolder.Updates
{
    public class GlobalStatisticsUpdateDefinition : DatabaseUpdateDefinition
    {
        private readonly Covid19ApiDbContext databaseContext;

        // ReSharper disable once SuggestBaseTypeForParameter
        public GlobalStatisticsUpdateDefinition(ILogger<GlobalStatisticsUpdateDefinition> logger,
            Covid19ApiDbContext databaseContext) : base(logger)
        {
            this.databaseContext = databaseContext;
        }

        public override int Version => 1;

        protected override async Task ExecuteAsync()
        {
            await this.databaseContext.Database.CreateCollectionIfNotExistsAsync(CollectionNames.GlobalStatistics);

            var fetchedAtIndex = Builders<GlobalStatistics>
                .IndexKeys
                .Descending(statistics => statistics.FetchedAt);

            var fetchedAtIndexModel = new CreateIndexModel<GlobalStatistics>(fetchedAtIndex, new CreateIndexOptions
            {
                Name = $"{CollectionNames.GlobalStatistics}_fetchedAt_descending" 
            });
            
            var collection = this.databaseContext.Database.GetCollection<GlobalStatistics>(CollectionNames.GlobalStatistics);

            await collection.Indexes.CreateOneAsync(fetchedAtIndexModel);
        }
    }
}