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

        public async Task AddAsync(ClosedCaseStats closedCaseStats)
        {
            var collection = this.context.Database.GetCollection<ClosedCaseStats>(CollectionName);
            
            var cursor = await collection.FindAsync(existingClosedCaseStats => existingClosedCaseStats.Id == closedCaseStats.Id);

            if (await cursor.FirstOrDefaultAsync() is null)
                await collection.InsertOneAsync(closedCaseStats);
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
    }
}