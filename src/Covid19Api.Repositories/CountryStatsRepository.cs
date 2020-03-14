using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Repositories.Mongo;
using MongoDB.Driver;

namespace Covid19Api.Repositories
{
    public class CountryStatsRepository
    {
        private const string CollectionName = "countrystats";
        private readonly Covid19DbContext context;

        public CountryStatsRepository(Covid19DbContext context)
        {
            this.context = context;
        }

        public Task AddManyAsync(IEnumerable<CountryStats> countryStats)
        {
            var collection = this.context.Database.GetCollection<CountryStats>(CollectionName);

            return collection.InsertManyAsync(countryStats, new InsertManyOptions
            {
                IsOrdered = false
            });
        }
    }
}