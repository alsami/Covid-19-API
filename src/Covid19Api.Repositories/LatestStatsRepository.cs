using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Repositories.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Covid19Api.Repositories
{
    public class LatestStatsRepository
    {
        private const string CollectionName = "lateststats";
        private readonly Covid19DbContext context;

        public LatestStatsRepository(Covid19DbContext context)
        {
            this.context = context;
        }

        public async Task StoreAsync(LatestStats latestStats)
        {
            var collection = this.context.Database.GetCollection<LatestStats>(CollectionName);

            var cursor =
                await collection.FindAsync(existingClosedCaseStats => existingClosedCaseStats.Id == latestStats.Id);

            if (await cursor.FirstOrDefaultAsync() is null)
            {
                await collection.InsertOneAsync(latestStats);
                return;
            }

            await collection.ReplaceOneAsync(stat => stat.Id == latestStats.Id, latestStats);
        }

        public async Task<LatestStats> MostRecentAsync()
        {
            var collection = this.context.Database.GetCollection<LatestStats>(CollectionName);

            var filter = Builders<LatestStats>.Filter.Empty;
            var sort = Builders<LatestStats>.Sort.Descending("FetchedAt");

            var cursor = await collection.FindAsync(filter, new FindOptions<LatestStats>
            {
                Sort = sort
            });

            return await cursor.FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<LatestStats>> HistoricalAsync(DateTime minFetchedAt)
        {
            var collection = this.context.Database.GetCollection<LatestStats>(CollectionName);
            var sort = Builders<LatestStats>.Sort.Descending("FetchedAt");

            var cursor = await collection.FindAsync(
                existingClosedCaseStats => existingClosedCaseStats.FetchedAt >= minFetchedAt,
                new FindOptions<LatestStats>
                {
                    Sort = sort
                });

            return await cursor.ToListAsync();
        }
    }
}