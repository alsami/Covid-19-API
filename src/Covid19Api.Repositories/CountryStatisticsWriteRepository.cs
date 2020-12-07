using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.Repositories.Extensions;
using MongoDB.Driver;

namespace Covid19Api.Repositories
{
    public class CountryStatisticsWriteRepository : ICountryStatisticsWriteRepository
    {
        private readonly Covid19ApiDbContext context;

        public CountryStatisticsWriteRepository(Covid19ApiDbContext context)
        {
            this.context = context;
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