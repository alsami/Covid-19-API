using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.Repositories.Extensions;
using MongoDB.Driver;
// ReSharper disable SpecifyStringComparison

namespace Covid19Api.Repositories
{
    public class CountryStatisticsRepository : ICountryStatisticsRepository
    {
        private readonly Covid19ApiDbContext context;

        public CountryStatisticsRepository(Covid19ApiDbContext context)
        {
            this.context = context;
        }

        public async Task<CountryStatistics> MostRecentAsync(string country)
        {
            var collection = this.GetCollection();

            var sort = Builders<CountryStatistics>.Sort
                .Descending(nameof(CountryStatistics.TotalCases))
                .Descending(nameof(CountryStatistics.FetchedAt));

            // ReSharper disable once SpecifyStringComparison
            var cursor = await collection.FindAsync(existingCountryStats =>
                    existingCountryStats.FetchedAt >= DateTime.UtcNow.Date.AddDays(-1) &&
                    existingCountryStats.Country.ToLower() == country.ToLower(),
                new FindOptions<CountryStatistics>
                {
                    Sort = sort,
                });

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CountryStatistics>> HistoricalAsync(DateTime minFetchedAt)
        {
            var collection = this.GetCollection();

            var sort = Builders<CountryStatistics>.Sort
                .Descending(nameof(CountryStatistics.TotalCases))
                .Descending(nameof(CountryStatistics.FetchedAt))
                .Ascending(nameof(CountryStatistics.Country));

            var cursor = await collection.FindAsync(
                existingCountryStats => existingCountryStats.FetchedAt >= minFetchedAt,
                new FindOptions<CountryStatistics>
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

        public async Task<IEnumerable<CountryStatistics>> HistoricalAsync(DateTime minFetchedAt, string country)
        {
            var collection = this.GetCollection();

            var sort = Builders<CountryStatistics>.Sort
                .Descending(nameof(CountryStatistics.TotalCases))
                .Descending(nameof(CountryStatistics.FetchedAt))
                .Ascending(nameof(CountryStatistics.Country));

            var cursor = await collection.FindAsync(
                existingCountryStats => existingCountryStats.FetchedAt >= minFetchedAt &&
                                        existingCountryStats.Country.ToLowerInvariant() == country.ToLowerInvariant(),
                new FindOptions<CountryStatistics>
                {
                    Sort = sort,
                });

            return await cursor.ToListAsync();
        }

        public async Task<IEnumerable<CountryStatistics>> HistoricalForDayAsync(DateTime minFetchedAt, string country)
        {
            var collection = this.GetCollection();

            var cursor = await collection.FindAsync(
                existingCountryStats => existingCountryStats.FetchedAt >= minFetchedAt &&
                                        existingCountryStats.Country.ToLowerInvariant() == country.ToLowerInvariant());

            return await cursor.ToListAsync();
        }

        public async Task StoreManyAsync(IEnumerable<CountryStatistics> countryStats)
        {
            var collection = this.GetCollection();

            var updates = countryStats.Select(currentStats =>
                {
                    var filterDefinition =
                        new FilterDefinitionBuilder<CountryStatistics>().Where(existingStats =>
                            existingStats.Id == currentStats.Id);

                    return new ReplaceOneModel<CountryStatistics>(filterDefinition, currentStats)
                    {
                        IsUpsert = true
                    };
                })
                .ToList();

            foreach (var chunk in updates.CreateChunks(50))
            {
                try
                {
                    await collection.BulkWriteAsync(chunk, new BulkWriteOptions
                    {
                        IsOrdered = false,
                    });
                }
                catch (Exception exception) when (exception is MongoBulkWriteException)
                {
                    // Might happen when having duplicate ids!
                }
            }
        }

        private IMongoCollection<CountryStatistics> GetCollection()
            => this.context.Database.GetCollection<CountryStatistics>(CollectionNames.CountryStatistics);
    }
}