using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using MongoDB.Driver;

namespace Covid19Api.Repositories
{
    public class GlobalStatisticsAggregatesReadRepository : IGlobalStatisticsAggregatesReadRepository
    {
        private readonly Covid19ApiDbContext context;

        public GlobalStatisticsAggregatesReadRepository(Covid19ApiDbContext context)
        {
            this.context = context;
        }

        public async Task<GlobalStatisticsAggregate?> FindAsync(int month, int year)
        {
            var collection = this.GetCollection();

            var cursor = await collection.FindAsync(aggregate => aggregate.Month == month && aggregate.Year == year);

            return await cursor.SingleOrDefaultAsync();
        }

        public async Task<IList<GlobalStatisticsAggregate>> FindInYearAsync(int year)
        {
            var collection = this.GetCollection();

            var cursor = await collection.FindAsync(aggregate => aggregate.Year == year);

            return await cursor.ToListAsync();
        }

        private IMongoCollection<GlobalStatisticsAggregate> GetCollection()
            => this.context.Database.GetCollection<GlobalStatisticsAggregate>(CollectionNames.GlobalStatisticAggregates);
    }
}