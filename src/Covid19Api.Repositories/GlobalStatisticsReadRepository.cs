using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using MongoDB.Driver;

namespace Covid19Api.Repositories;

public class GlobalStatisticsReadRepository : IGlobalStatisticsReadRepository
{
    private readonly Covid19ApiDbContext context;

    public GlobalStatisticsReadRepository(Covid19ApiDbContext context)
    {
        this.context = context;
    }

    public Task<GlobalStatistics> LoadCurrentAsync()
    {
        var collection = this.GetCollection();
            
        return collection.Find(_ => true).SortByDescending(statistics => statistics.FetchedAt).Limit(1).SingleAsync();
    }

    public async Task<IEnumerable<GlobalStatistics>> HistoricalAsync(DateTime minFetchedAt)
    {
        var collection = this.GetCollection();

        var sort = Builders<GlobalStatistics>
            .Sort
            .Descending(nameof(GlobalStatistics.FetchedAt));

        var cursor = await collection.FindAsync(
            globalStatistics => globalStatistics.FetchedAt >= minFetchedAt, new FindOptions<GlobalStatistics> {Sort = sort});

        return await cursor.ToListAsync();
    }

    public async Task<GlobalStatistics?> FindInRangeAsync(DateTime inclusiveStart, DateTime inclusiveEnd)
    {
        var collection = this.GetCollection();

        var leftFilter = Builders<GlobalStatistics>.Filter.Where(global => global.FetchedAt >= inclusiveStart);
        var rightFilter = Builders<GlobalStatistics>.Filter.Where(global => global.FetchedAt <= inclusiveEnd);
        var combinedFilter = leftFilter & rightFilter;
        var sort = Builders<GlobalStatistics>.Sort.Descending(global => global.FetchedAt);

        return await collection.Find(combinedFilter).Sort(sort).Limit(1).FirstOrDefaultAsync();
    }

    private IMongoCollection<GlobalStatistics> GetCollection()
        => this.context.Database.GetCollection<GlobalStatistics>(CollectionNames.GlobalStatistics);
}