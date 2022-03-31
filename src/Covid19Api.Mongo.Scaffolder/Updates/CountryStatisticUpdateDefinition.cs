using Covid19Api.Domain;
using Covid19Api.Mongo.Scaffolder.Abstractions;
using Covid19Api.Mongo.Scaffolder.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Covid19Api.Mongo.Scaffolder.Updates;

// ReSharper disable once UnusedType.Global
public class CountryStatisticUpdateDefinition : DatabaseUpdateDefinition
{
    private readonly Covid19ApiDbContext databaseContext;

    // ReSharper disable once SuggestBaseTypeForParameter
    public CountryStatisticUpdateDefinition(ILogger<CountryStatisticUpdateDefinition> logger,
        Covid19ApiDbContext databaseContext) : base(logger)
    {
        this.databaseContext = databaseContext;
    }

    public override int Version => 0;

    protected override async Task ExecuteAsync()
    {
        await this.databaseContext.Database.CreateCollectionIfNotExistsAsync(CollectionNames.CountryStatistics);

        var collection =
            this.databaseContext.Database.GetCollection<CountryStatistic>(CollectionNames.CountryStatistics);

        await collection.Indexes.CreateManyAsync(new[]
        {
            CreateTotalCasesFetchedAtIndexModel(),
            CreateTotalCasesFetchedAtCountryIndexModel(),
            CreatFetchedAtIndexModel()
        });
    }

    private static CreateIndexModel<CountryStatistic> CreatFetchedAtIndexModel()
    {
        var fetchedAt = Builders<CountryStatistic>
            .IndexKeys
            .Ascending(statistics => statistics.FetchedAt);

        var fetchedAtIndexModel = new CreateIndexModel<CountryStatistic>(fetchedAt, new CreateIndexOptions
        {
            Name = $"{CollectionNames.CountryStatistics}_fetchedAt"
        });
        return fetchedAtIndexModel;
    }

    private static CreateIndexModel<CountryStatistic> CreateTotalCasesFetchedAtIndexModel()
    {
        var totalCasesFetchedAtIndex = Builders<CountryStatistic>
            .IndexKeys
            .Descending(statistics => statistics.TotalCases)
            .Descending(statistics => statistics.FetchedAt);

        var totalCasesFetchedAtIndexModel = new CreateIndexModel<CountryStatistic>(totalCasesFetchedAtIndex,
            new CreateIndexOptions
            {
                Name = $"{CollectionNames.CountryStatistics}_totalCases_fetchedAt_descending"
            });
        return totalCasesFetchedAtIndexModel;
    }

    private static CreateIndexModel<CountryStatistic> CreateTotalCasesFetchedAtCountryIndexModel()
    {
        var totalCasesFetchedAtCountryIndex = Builders<CountryStatistic>
            .IndexKeys
            .Descending(statistics => statistics.TotalCases)
            .Descending(statistics => statistics.FetchedAt)
            .Ascending(statistics => statistics.Country);

        var totalCasesFetchedAtCountryIndexModel = new CreateIndexModel<CountryStatistic>(
            totalCasesFetchedAtCountryIndex,
            new CreateIndexOptions
            {
                Name = $"{CollectionNames.CountryStatistics}_totalCases_fetchedAt_descending_country_ascending"
            });
        return totalCasesFetchedAtCountryIndexModel;
    }
}