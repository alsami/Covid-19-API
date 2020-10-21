using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using MongoDB.Driver;

namespace Covid19Api.Repositories
{
    public class GlobalStatisticsRepository : IGlobalStatisticsRepository
    {
        private readonly Covid19ApiDbContext context;

        public GlobalStatisticsRepository(Covid19ApiDbContext context)
        {
            this.context = context;
        }

        public async Task StoreAsync(GlobalStatistics globalStatistics)
        {
            var collection = this.GetCollection();

            await collection.ReplaceOneAsync(stats => stats.Id == globalStatistics.Id, globalStatistics,
                new ReplaceOptions
                {
                    IsUpsert = true
                });
        }

        public async Task<IEnumerable<GlobalStatistics>> HistoricalAsync(DateTime minFetchedAt)
        {
            var collection = this.GetCollection();

            var sort = Builders<GlobalStatistics>
                .Sort
                .Descending(nameof(GlobalStatistics.FetchedAt));

            var cursor = await collection.FindAsync(
                existingClosedCaseStats => existingClosedCaseStats.FetchedAt >= minFetchedAt,
                new FindOptions<GlobalStatistics>
                {
                    Sort = sort
                });

            return await cursor.ToListAsync();
        }

        public async Task<IEnumerable<GlobalStatistics>> HistoricalForDayAsync(DateTime minFetchedAt)
        {
            var collection = this.GetCollection();

            var cursor = await collection.FindAsync(
                existingCountryStats => existingCountryStats.FetchedAt >= minFetchedAt);

            var all = await cursor.ToListAsync();

            return all.OrderBy(entry => entry.FetchedAt);
        }

        public async Task<GlobalStatistics?> FindInRangeAsync(DateTime inclusiveStart, DateTime inclusiveEnd)
        {
            var collection = this.GetCollection();

            var leftFilter = Builders<GlobalStatistics>.Filter.Where(global => global.FetchedAt >= inclusiveStart);
            var rightFilter = Builders<GlobalStatistics>.Filter.Where(global => global.FetchedAt <= inclusiveEnd);
            var combinedFilter = leftFilter & rightFilter;
            var sort = Builders<GlobalStatistics>.Sort.Descending(global => global.FetchedAt);

            var cursor = await collection.FindAsync(combinedFilter, new FindOptions<GlobalStatistics>
            {
                Sort = sort
            });

            return await cursor.FirstOrDefaultAsync();
        }

        private IMongoCollection<GlobalStatistics> GetCollection()
            => this.context.Database.GetCollection<GlobalStatistics>(CollectionNames.GlobalStatistics);
    }
}