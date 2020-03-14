using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Repositories.Mongo;

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

        public Task AddAsync(LatestStats latestStats)
        {
            var collection = this.context.Database.GetCollection<LatestStats>(CollectionName);

            return collection.InsertOneAsync(latestStats);
        }
    }
}