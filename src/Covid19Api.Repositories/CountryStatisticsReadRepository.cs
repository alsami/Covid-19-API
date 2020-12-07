using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using MongoDB.Driver;

// ReSharper disable SpecifyStringComparison

namespace Covid19Api.Repositories
{
    public class CountryStatisticsReadRepository : ICountryStatisticsReadRepository
    {
        private readonly Covid19ApiDbContext context;

        public CountryStatisticsReadRepository(Covid19ApiDbContext context)
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
            var cursor = await collection.FindAsync(statistics =>
                    statistics.FetchedAt >= DateTime.UtcNow.Date.AddDays(-1) &&
                    statistics.Country.ToLower() == country.ToLower() &&
                    statistics.Key == CollectionNames.CountryStatistics,
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
                statistics => statistics.FetchedAt >= minFetchedAt &&
                              statistics.Key == CollectionNames.CountryStatistics,
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
                statistics => statistics.FetchedAt >= minFetchedAt &&
                              statistics.Country.ToLowerInvariant() == country.ToLowerInvariant() &&
                              statistics.Key == CollectionNames.CountryStatistics,
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
                statistics => statistics.FetchedAt >= minFetchedAt &&
                              statistics.Country.ToLowerInvariant() == country.ToLowerInvariant() &&
                              statistics.Key == CollectionNames.CountryStatistics);

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

            var keyFilter =
                Builders<CountryStatistic>.Filter.Where(statistics =>
                    statistics.Key == CollectionNames.CountryStatistics);

            var sort = Builders<CountryStatistic>.Sort.Descending(statistics => statistics.FetchedAt);

            var filter = countryFilter & startFilter & endFilter & keyFilter;

            var cursor = await collection.FindAsync(filter, new FindOptions<CountryStatistic>
            {
                Sort = sort
            });

            return await cursor.FirstOrDefaultAsync();
        }

        private IMongoCollection<CountryStatistic> GetCollection()
            => this.context.Database.GetCollection<CountryStatistic>(CollectionNames.CountryStatistics);
    }
}