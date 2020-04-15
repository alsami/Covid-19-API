using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Repositories.Mongo;
using MongoDB.Driver;

// ReSharper disable SpecifyStringComparison

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

            var cursor = await collection.FindAsync(
                existingCountryStats => existingCountryStats.FetchedAt >= DateTime.UtcNow.Date.AddDays(-1),
                new FindOptions<CountryStats>
                {
                    Sort = sort,
                });

            var all = await cursor.ToListAsync();

            var onlyLatestEntries = all.GroupBy(countryStats => countryStats.Country)
                .SelectMany(grouping => grouping.Take(1));

            return onlyLatestEntries;
        }

        public async Task<CountryStats> MostRecentAsync(string country)
        {
            var collection = this.context.Database.GetCollection<CountryStats>(CollectionName);

            var sort = Builders<CountryStats>.Sort
                .Descending("TotalCases")
                .Descending("FetchedAt");

            // ReSharper disable once SpecifyStringComparison
            var cursor = await collection.FindAsync(existingCountryStats =>
                    existingCountryStats.FetchedAt >= DateTime.UtcNow.Date.AddDays(-1) &&
                    existingCountryStats.Country.ToLower() == country.ToLower(),
                new FindOptions<CountryStats>
                {
                    Sort = sort,
                });

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CountryStats>> HistoricalAsync(DateTime minFetchedAt)
        {
            var collection = this.context.Database.GetCollection<CountryStats>(CollectionName);

            var sort = Builders<CountryStats>.Sort
                .Descending("TotalCases")
                .Descending("FetchedAt")
                .Ascending("Country");

            var cursor = await collection.FindAsync(
                existingCountryStats => existingCountryStats.FetchedAt >= minFetchedAt, new FindOptions<CountryStats>
                {
                    Sort = sort,
                });

            var all = await cursor.ToListAsync();

            return all.GroupBy(countryStats => new
                {
                    countryStats.FetchedAt.Date,
                    countryStats.Country
                })
                .SelectMany(grouping => grouping.Take(1));
        }

        public async Task<IEnumerable<CountryStats>> HistoricalAsync(DateTime minFetchedAt, string country)
        {
            var collection = this.context.Database.GetCollection<CountryStats>(CollectionName);

            var sort = Builders<CountryStats>.Sort
                .Descending("TotalCases")
                .Descending("FetchedAt")
                .Ascending("Country");

            var cursor = await collection.FindAsync(
                existingCountryStats => existingCountryStats.FetchedAt >= minFetchedAt &&
                                        existingCountryStats.Country.ToLower() == country.ToLower(),
                new FindOptions<CountryStats>
                {
                    Sort = sort,
                });

            return await cursor.ToListAsync();
        }

        public async Task<IEnumerable<CountryStats>> HistoricalForDayAsync(DateTime minFetchedAt, string country)
        {
            var collection = this.context.Database.GetCollection<CountryStats>(CollectionName);

            var sort = Builders<CountryStats>
                .Sort
                .Descending("TotalCases")
                .Descending("FetchedAt");

            var cursor = await collection.FindAsync(
                existingCountryStats => existingCountryStats.FetchedAt >= minFetchedAt &&
                                        existingCountryStats.Country.ToLower() == country.ToLower(),
                new FindOptions<CountryStats>
                {
                    Sort = sort,
                });

            var all = await cursor.ToListAsync();

            var onlyLatestEntries = all.GroupBy(countryStats => countryStats.FetchedAt.Date)
                .SelectMany(grouping => grouping.Take(1));

            return onlyLatestEntries.OrderBy(entry => entry.FetchedAt);
        }

        public Task StoreAsync(IEnumerable<CountryStats> countryStats)
        {
            var collection = this.context.Database.GetCollection<CountryStats>(CollectionName);

            var updates = countryStats.Select(currentStats =>
            {
                var filterDefinition = new FilterDefinitionBuilder<CountryStats>().Where(existingStats => existingStats.Id == currentStats.Id);

                return new ReplaceOneModel<CountryStats>(filterDefinition, currentStats)
                {
                    IsUpsert = true
                };
            });

            return collection.BulkWriteAsync(updates, new BulkWriteOptions
            {
                IsOrdered = false,
            });
        }
    }
}