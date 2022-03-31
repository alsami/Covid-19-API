using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using MongoDB.Driver;

namespace Covid19Api.Repositories;

public class GlobalStatisticsWriteRepository : IGlobalStatisticsWriteRepository
{
    private readonly Covid19ApiDbContext context;

    public GlobalStatisticsWriteRepository(Covid19ApiDbContext context)
    {
        this.context = context;
    }
        
    public async Task StoreAsync(GlobalStatistics globalStatistics)
    {
        var collection = this.GetCollection();

        await collection.ReplaceOneAsync(stats => stats.Id == globalStatistics.Id, globalStatistics,
            new ReplaceOptions
            {
                IsUpsert = true
            });
    }
        
    private IMongoCollection<GlobalStatistics> GetCollection()
        => this.context.Database.GetCollection<GlobalStatistics>(CollectionNames.GlobalStatistics);
}