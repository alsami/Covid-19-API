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

        public async Task<CountryStatistic> MostRecentAsync(string country)
        {
            var collection = this.GetCollection();

            var sort = Builders<CountryStatistic>.Sort
                .Descending(nameof(CountryStatistic.TotalCases))
                .Descending(nameof(CountryStatistic.FetchedAt));

            // ReSharper disable once SpecifyStringComparison
            var cursor = await collection.FindAsync(existingCountryStats =>
                    existingCountryStats.FetchedAt >= DateTime.UtcNow.Date.AddDays(-1) &&
                    existingCountryStats.Country.ToLower() == country.ToLower(),
                new FindOptions<CountryStatistic>
                {
                    Sort = sort,
                });

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CountryStatistic>> HistoricalAsync(DateTime minFetchedAt)
        {
            var collection = this.GetCollection();

            var sort = Builders<CountryStatistic>.Sort
                .Descending(nameof(CountryStatistic.TotalCases))
                .Descending(nameof(CountryStatistic.FetchedAt))
                .Ascending(nameof(CountryStatistic.Country));

            var cursor = await collection.FindAsync(
                existingCountryStats => existingCountryStats.FetchedAt >= minFetchedAt,
                new FindOptions<CountryStatistic>
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

        public async Task<IEnumerable<CountryStatistic>> HistoricalAsync(DateTime minFetchedAt, string country)
        {
            var collection = this.GetCollection();

            var sort = Builders<CountryStatistic>.Sort
                .Descending(nameof(CountryStatistic.TotalCases))
                .Descending(nameof(CountryStatistic.FetchedAt))
                .Ascending(nameof(CountryStatistic.Country));

            var cursor = await collection.FindAsync(
                existingCountryStats => existingCountryStats.FetchedAt >= minFetchedAt &&
                                        existingCountryStats.Country.ToLowerInvariant() == country.ToLowerInvariant(),
                new FindOptions<CountryStatistic>
                {
                    Sort = sort,
                });

            return await cursor.ToListAsync();
        }

        public async Task<IEnumerable<CountryStatistic>> HistoricalForDayAsync(DateTime minFetchedAt, string country)
        {
            var collection = this.GetCollection();

            var cursor = await collection.FindAsync(
                existingCountryStats => existingCountryStats.FetchedAt >= minFetchedAt &&
                                        existingCountryStats.Country.ToLowerInvariant() == country.ToLowerInvariant());

            return await cursor.ToListAsync();
        }

        public async Task<CountryStatistic?> FindInRangeAsync(string country, DateTime inclusiveStart,
            DateTime exclusiveEnd)
        {
            var collection = this.GetCollection();

            var countryFilter =
                Builders<CountryStatistic>.Filter.Where(
                    statistics => statistics.Country.ToLower() == country.ToLower());

            var startFilter =
                Builders<CountryStatistic>.Filter.Where(
                    statistics => statistics.FetchedAt >= inclusiveStart);

            var endFilter =
                Builders<CountryStatistic>.Filter.Where(
                    statistics => statistics.FetchedAt <= exclusiveEnd);

            var sort = Builders<CountryStatistic>.Sort.Descending(statistics => statistics.FetchedAt);

            var filter = countryFilter & startFilter & endFilter;

            var cursor = await collection.FindAsync(filter, new FindOptions<CountryStatistic>
            {
                Sort = sort
            });

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task StoreManyAsync(IEnumerable<CountryStatistic> countryStats)
        {
            var collection = this.GetCollection();

            var updates = countryStats.Select(currentStats =>
                {
                    var filterDefinition =
                        new FilterDefinitionBuilder<CountryStatistic>().Where(existingStats =>
                            existingStats.Id == currentStats.Id);

                    return new ReplaceOneModel<CountryStatistic>(filterDefinition, currentStats)
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

        private IMongoCollection<CountryStatistic> GetCollection()
            => this.context.Database.GetCollection<CountryStatistic>(CollectionNames.CountryStatistics);
    }
}