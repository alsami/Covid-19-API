using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Repositories.Mongo;
using MongoDB.Driver;

namespace Covid19Api.Repositories
{
    public class ClosedCasesRepository
    {
        private const string CollectionName = "closedcasestats";
        private readonly Covid19DbContext context;

        public ClosedCasesRepository(Covid19DbContext context)
        {
            this.context = context;
        }

        public async Task StoreAsync(ClosedCaseStats closedCaseStats)
        {
            var collection = this.context.Database.GetCollection<ClosedCaseStats>(CollectionName);

            var cursor = await collection.FindAsync(existingClosedCaseStats =>
                existingClosedCaseStats.Id == closedCaseStats.Id);

            if (await cursor.FirstOrDefaultAsync() is null)
            {
                await collection.InsertOneAsync(closedCaseStats);
                return;
            }

            await collection.ReplaceOneAsync(stat => stat.Id == closedCaseStats.Id, closedCaseStats);
        }

        public async Task<ClosedCaseStats> MostRecentAsync()
        {
            var collection = this.context.Database.GetCollection<ClosedCaseStats>(CollectionName);

            var filter = Builders<ClosedCaseStats>.Filter.Empty;
            var sort = Builders<ClosedCaseStats>.Sort.Descending("FetchedAt");

            var cursor = await collection.FindAsync(filter, new FindOptions<ClosedCaseStats>
            {
                Sort = sort
            });

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ClosedCaseStats>> HistoricalAsync(DateTime minFetchedAt)
        {
            var collection = this.context.Database.GetCollection<ClosedCaseStats>(CollectionName);
            var sort = Builders<ClosedCaseStats>.Sort.Descending("FetchedAt");

            var cursor = await collection.FindAsync(
                existingClosedCaseStats => existingClosedCaseStats.FetchedAt >= minFetchedAt,
                new FindOptions<ClosedCaseStats>
                {
                    Sort = sort
                });

            return await cursor.ToListAsync();
        }
        
        public async Task<IEnumerable<ClosedCaseStats>> HistoricalForDayAsync(DateTime minFetchedAt)
        {
            var collection = this.context.Database.GetCollection<ClosedCaseStats>(CollectionName);

            var sort = Builders<ClosedCaseStats>
                .Sort
                .Descending("FetchedAt")
                .Descending("Total");

            var cursor = await collection.FindAsync(
                existingCountryStats => existingCountryStats.FetchedAt >= minFetchedAt,
                new FindOptions<ClosedCaseStats>
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