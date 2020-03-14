using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Repositories.Mongo;

namespace Covid19Api.Repositories
{
    public class ActiveCasesRepository
    {
        private const string CollectionName = "activecasestats";
        private readonly Covid19DbContext context;

        public ActiveCasesRepository(Covid19DbContext context)
        {
            this.context = context;
        }

        public Task AddAsync(ActiveCaseStats activeCaseStats)
        {
            var collection = this.context.Database.GetCollection<ActiveCaseStats>(CollectionName);

            return collection.InsertOneAsync(activeCaseStats);
        }
    }
}