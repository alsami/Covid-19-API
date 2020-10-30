using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.Repositories.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Covid19Api.Repositories
{
    public class CountryStatisticsAggregatesRepository : ICountryStatisticsAggregatesRepository
    {
        private readonly ILogger<CountryStatisticsAggregatesRepository> logger;
        private readonly Covid19ApiDbContext context;


        public CountryStatisticsAggregatesRepository(ILogger<CountryStatisticsAggregatesRepository> logger, Covid19ApiDbContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public Task StoreAsync(CountryStatisticsAggregate countryStatisticsAggregate)
        {
            var collection = this.GetCollection();

            return collection.ReplaceOneAsync(existing => existing.Id == countryStatisticsAggregate.Id,
                countryStatisticsAggregate, new ReplaceOptions
                {
                    IsUpsert = true
                });
        }

        public async Task StoreManyAsync(IEnumerable<CountryStatisticsAggregate> countryStatisticsAggregates)
        {
            var collection = this.GetCollection();

            var replacements = countryStatisticsAggregates.Select(currentStats =>
                {
                    var filterDefinition =
                        new FilterDefinitionBuilder<CountryStatisticsAggregate>().Where(existingStats =>
                            existingStats.Id == currentStats.Id);

                    return new ReplaceOneModel<CountryStatisticsAggregate>(filterDefinition, currentStats)
                    {
                        IsUpsert = true
                    };
                })
                .ToList();

            foreach (var replacementsChunk in replacements.CreateChunks(50))
            {
                try
                {
                    await collection.BulkWriteAsync(replacementsChunk, new BulkWriteOptions
                    {
                        IsOrdered = false,
                    });
                }
                catch (Exception exception) when (exception is MongoBulkWriteException)
                {
                    this.logger.LogWarning(exception, "Error while bulk-writing country-statistics");
                }
            }
        }

        public async Task<CountryStatisticsAggregate?> FindAsync(string country, int month, int year)
        {
            // ReSharper disable once SpecifyStringComparison
            var countryFilter =
                Builders<CountryStatisticsAggregate>.Filter.Where(statistics =>
                    statistics.Country.ToLower() == country.ToLower());

            var monthFilter =
                Builders<CountryStatisticsAggregate>.Filter.Where(statistics => statistics.Month == month);

            var yearFilter =
                Builders<CountryStatisticsAggregate>.Filter.Where(statistics => statistics.Year == year);

            var filter = countryFilter & monthFilter & yearFilter;

            var collection = this.GetCollection();

            var cursor = await collection.FindAsync(filter);

            return await cursor.SingleOrDefaultAsync();
        }

        public async Task<IList<CountryStatisticsAggregate>> FindForCountryInYearAsync(string country, int year)
        {
            // ReSharper disable once SpecifyStringComparison
            var countryFilter =
                Builders<CountryStatisticsAggregate>.Filter.Where(statistics =>
                    statistics.Country.ToLower() == country.ToLower());

            var yearFilter =
                Builders<CountryStatisticsAggregate>.Filter.Where(statistics => statistics.Year == year);

            var filter = countryFilter & yearFilter;

            var collection = this.GetCollection();

            var cursor = await collection.FindAsync(filter);

            return await cursor.ToListAsync();
        }

        private IMongoCollection<CountryStatisticsAggregate> GetCollection()
            => this.context.Database.GetCollection<CountryStatisticsAggregate>(CollectionNames
                .CountryStatisticsAggregates);
    }
}