using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.Repositories.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Covid19Api.Repositories;

public class CountryStatisticsAggregatesWriteRepository : ICountryStatisticsAggregatesWriteRepository
{
    private readonly ILogger<CountryStatisticsAggregatesWriteRepository> logger;
    private readonly Covid19ApiDbContext context;

    public CountryStatisticsAggregatesWriteRepository(ILogger<CountryStatisticsAggregatesWriteRepository> logger, Covid19ApiDbContext context)
    {
        this.logger = logger;
        this.context = context;
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
        
    private IMongoCollection<CountryStatisticsAggregate> GetCollection()
        => this.context.Database.GetCollection<CountryStatisticsAggregate>(CollectionNames.CountryStatisticsAggregates);
}