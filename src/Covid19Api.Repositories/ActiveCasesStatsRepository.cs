using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Repositories.Mongo;
using MongoDB.Driver;

namespace Covid19Api.Repositories
{
    public class ActiveCasesStatsRepository
    {
        private const string CollectionName = "activecasestats";
        private readonly Covid19DbContext context;

        public ActiveCasesStatsRepository(Covid19DbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(ActiveCaseStats activeCaseStats)
        {
            var collection = this.context.Database.GetCollection<ActiveCaseStats>(CollectionName);

            var cursor = await collection.FindAsync(activeCast => activeCast.Id == activeCaseStats.Id);

            if (await cursor.FirstOrDefaultAsync() is null)
                await collection.InsertOneAsync(activeCaseStats);
        }

        public async Task<ActiveCaseStats> MostRecentAsync()
        {
            var collection = this.context.Database.GetCollection<ActiveCaseStats>(CollectionName);

            var filter = Builders<ActiveCaseStats>.Filter.Empty;
            var sort = Builders<ActiveCaseStats>.Sort.Descending("FetchedAt");

            var cursor = await collection.FindAsync(filter, new FindOptions<ActiveCaseStats>
            {
                Sort = sort
            });

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ActiveCaseStats>> HistoricalAsync(DateTime minFetchedAt)
        {
            var collection = this.context.Database.GetCollection<ActiveCaseStats>(CollectionName);
            var sort = Builders<ActiveCaseStats>.Sort.Descending("FetchedAt");
            
            var cursor = await collection.FindAsync(existingActiveCaseStats => existingActiveCaseStats.FetchedAt >= minFetchedAt, new FindOptions<ActiveCaseStats>
            {
                Sort = sort
            });

            return await cursor.ToListAsync();
        }
    }
}