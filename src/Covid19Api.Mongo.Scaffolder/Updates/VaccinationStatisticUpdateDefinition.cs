using Covid19Api.Domain;
using Covid19Api.Mongo.Scaffolder.Abstractions;
using Covid19Api.Mongo.Scaffolder.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Covid19Api.Mongo.Scaffolder.Updates;

// ReSharper disable once UnusedType.Global
public class VaccinationStatisticUpdateDefinition : DatabaseUpdateDefinition
{
    private readonly Covid19ApiDbContext databaseContext;
        
    // ReSharper disable once SuggestBaseTypeForParameter
    public VaccinationStatisticUpdateDefinition(ILogger<VaccinationStatisticUpdateDefinition> logger, Covid19ApiDbContext databaseContext) : base(logger)
    {
        this.databaseContext = databaseContext;
    }

    public override int Version => 4;
    protected override async Task ExecuteAsync()
    {
        await this.databaseContext.Database.CreateCollectionIfNotExistsAsync(CollectionNames.VaccinationStatistic);
            
        var countryIndex = Builders<VaccinationStatistic>
            .IndexKeys
            .Ascending(statistics => statistics.Country);
            
        var countryIndexModel = new CreateIndexModel<VaccinationStatistic>(countryIndex, new CreateIndexOptions
        {
            Name = $"{CollectionNames.VaccinationStatistic}_country_ascending",
            Unique = true
        });
            
        var countryCodeIndex = Builders<VaccinationStatistic>
            .IndexKeys
            .Ascending(statistics => statistics.CountyCode);
            
        var countryCodeIndexModel = new CreateIndexModel<VaccinationStatistic>(countryCodeIndex, new CreateIndexOptions
        {
            Name = $"{CollectionNames.VaccinationStatistic}_country_code_ascending",
            Unique = true
        });
            
        var collection =
            this.databaseContext.Database.GetCollection<VaccinationStatistic>(CollectionNames.VaccinationStatistic);

        await collection.Indexes.CreateManyAsync(new []{ countryIndexModel, countryCodeIndexModel });
    }
}