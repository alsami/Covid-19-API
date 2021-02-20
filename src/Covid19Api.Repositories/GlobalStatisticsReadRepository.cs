using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Domain.Constants;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using MongoDB.Driver;

namespace Covid19Api.Repositories
{
    public class GlobalStatisticsReadRepository : IGlobalStatisticsReadRepository
    {
        private readonly Covid19ApiDbContext context;

        public GlobalStatisticsReadRepository(Covid19ApiDbContext context)
        {
            this.context = context;
        }

        public Task<GlobalStatistics> LoadCurrentAsync()
        {
            var collection = this.GetCollection();
            
            var keyFilter = Builders<GlobalStatistics>.Filter.Where(global => global.Key == EntityKeys.GlobalStatistics);

            return collection.Find(keyFilter).SortByDescending(statistics => statistics.FetchedAt).Limit(1).SingleAsync();
        }

        public async Task<IEnumerable<GlobalStatistics>> HistoricalAsync(DateTime minFetchedAt)
        {
            var collection = this.GetCollection();

            var sort = Builders<GlobalStatistics>
                .Sort
                .Descending(nameof(GlobalStatistics.FetchedAt));

            var cursor = await collection.FindAsync(
                globalStatistics => globalStatistics.FetchedAt >= minFetchedAt &&
                                    globalStatistics.Key == EntityKeys.GlobalStatistics,
                new FindOptions<GlobalStatistics>
                {
                    Sort = sort
                });

            return await cursor.ToListAsync();
        }

        public async Task<GlobalStatistics?> FindInRangeAsync(DateTime inclusiveStart, DateTime inclusiveEnd)
        {
            var collection = this.GetCollection();

            var leftFilter = Builders<GlobalStatistics>.Filter.Where(global => global.FetchedAt >= inclusiveStart);
            var rightFilter = Builders<GlobalStatistics>.Filter.Where(global => global.FetchedAt <= inclusiveEnd);
            var keyFilter = Builders<GlobalStatistics>.Filter.Where(global => global.Key == EntityKeys.GlobalStatistics);
            var combinedFilter = leftFilter & rightFilter & keyFilter;
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