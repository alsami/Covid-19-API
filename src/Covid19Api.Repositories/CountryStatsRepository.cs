using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<CountryStats>> MostRecentAsync()
        {
            var collection = this.context.Database.GetCollection<CountryStats>(CollectionName);

            var sort = Builders<CountryStats>.Sort.Descending("TotalCases");

            var cursor = await collection.FindAsync(existingCountryStats => existingCountryStats.FetchedAt >= DateTime.UtcNow.Date.AddDays(-1), new FindOptions<CountryStats>
            {
                Sort = sort,
            });

            var all = await cursor.ToListAsync();

            var onlyLatestEntries = all.GroupBy(countryStats => countryStats.Country)
                .SelectMany(grouping => grouping.Take(1));

            return onlyLatestEntries;
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