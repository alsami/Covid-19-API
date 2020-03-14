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

        public Task AddAsync(ActiveCaseStats activeCaseStats)
        {
            var collection = this.context.Database.GetCollection<ActiveCaseStats>(CollectionName);

            return collection.InsertOneAsync(activeCaseStats);
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
    }
}