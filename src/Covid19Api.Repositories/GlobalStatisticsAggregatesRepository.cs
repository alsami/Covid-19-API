using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using MongoDB.Driver;

namespace Covid19Api.Repositories
{
    public class GlobalStatisticsAggregatesRepository : IGlobalStatisticsAggregatesRepository
    {
        private readonly Covid19ApiDbContext context;

        public GlobalStatisticsAggregatesRepository(Covid19ApiDbContext context)
        {
            this.context = context;
        }

        public Task StoreAsync(GlobalStatisticsAggregate globalStatisticsAggregate)
        {
            var collection = this.GetCollection();

            var filter = Builders<GlobalStatisticsAggregate>
                .Filter.Where(aggregate => aggregate.Id == globalStatisticsAggregate.Id);

            return collection.ReplaceOneAsync(filter,
                globalStatisticsAggregate, new ReplaceOptions
                {
                    IsUpsert = true
                });
        }

        public async Task<GlobalStatisticsAggregate?> FindAsync(int month, int year)
        {
            var collection = this.GetCollection();

            var cursor = await collection.FindAsync(aggregate => aggregate.Month == month && aggregate.Year == year);

            return await cursor.SingleOrDefaultAsync();
        }

        private IMongoCollection<GlobalStatisticsAggregate> GetCollection()
            => this.context.Database.GetCollection<GlobalStatisticsAggregate>(CollectionNames
                .GlobalStatisticsAggregates);
    }
}