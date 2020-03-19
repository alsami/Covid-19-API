using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Repositories.Mongo;
using MongoDB.Driver;

namespace Covid19Api.Repositories
{
    public class GlobalStatsRepository
    {
        // the type has been renamed but the collection not since azure cosmos db does not allow renaming collections
        private const string CollectionName = "lateststats";
        private readonly Covid19DbContext context;

        public GlobalStatsRepository(Covid19DbContext context)
        {
            this.context = context;
        }

        public async Task StoreAsync(GlobalStats globalStats)
        {
            var collection = this.context.Database.GetCollection<GlobalStats>(CollectionName);

            var cursor =
                await collection.FindAsync(existingClosedCaseStats => existingClosedCaseStats.Id == globalStats.Id);

            if (await cursor.FirstOrDefaultAsync() is null)
            {
                await collection.InsertOneAsync(globalStats);
                return;
            }

            await collection.ReplaceOneAsync(stat => stat.Id == globalStats.Id, globalStats);
        }

        public async Task<GlobalStats> MostRecentAsync()
        {
            var collection = this.context.Database.GetCollection<GlobalStats>(CollectionName);

            var filter = Builders<GlobalStats>.Filter.Empty;
            var sort = Builders<GlobalStats>.Sort.Descending("FetchedAt");

            var cursor = await collection.FindAsync(filter, new FindOptions<GlobalStats>
            {
                Sort = sort
            });

            return await cursor.FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<GlobalStats>> HistoricalAsync(DateTime minFetchedAt)
        {
            var collection = this.context.Database.GetCollection<GlobalStats>(CollectionName);
            var sort = Builders<GlobalStats>.Sort.Descending("FetchedAt");

            var cursor = await collection.FindAsync(
                existingClosedCaseStats => existingClosedCaseStats.FetchedAt >= minFetchedAt,
                new FindOptions<GlobalStats>
                {
                    Sort = sort
                });

            return await cursor.ToListAsync();
        }
        
        public async Task<IEnumerable<GlobalStats>> HistoricalForDayAsync(DateTime minFetchedAt)
        {
            var collection = this.context.Database.GetCollection<GlobalStats>(CollectionName);

            var sort = Builders<GlobalStats>
                .Sort
                .Descending("FetchedAt")
                .Descending("Total");

            var cursor = await collection.FindAsync(
                existingCountryStats => existingCountryStats.FetchedAt >= minFetchedAt,
                new FindOptions<GlobalStats>
                {
                    Sort = sort,
                });

            var all = await cursor.ToListAsync();

            var onlyLatestEntries = all.GroupBy(countryStats => countryStats.FetchedAt.Date)
                .SelectMany(grouping => grouping.Take(1));

            return onlyLatestEntries.OrderBy(entry => entry.FetchedAt);
        }
    }
}