using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Covid19Api.Repositories;

public class CountryStatisticsAggregatesReadRepository : ICountryStatisticsAggregatesReadRepository
{
    private readonly ILogger<CountryStatisticsAggregatesReadRepository> logger;
    private readonly Covid19ApiDbContext context;


    public CountryStatisticsAggregatesReadRepository(ILogger<CountryStatisticsAggregatesReadRepository> logger, Covid19ApiDbContext context)
    {
        this.logger = logger;
        this.context = context;
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
        var countryFilter = Builders<CountryStatisticsAggregate>
            .Filter
            .Where(statistics => statistics.Country.ToLower() == country.ToLower());

        var yearFilter = Builders<CountryStatisticsAggregate>.Filter.Where(statistics => statistics.Year == year);

        var filter = countryFilter & yearFilter;

        var collection = this.GetCollection();

        var cursor = await collection.FindAsync(filter);

        return await cursor.ToListAsync();
    }

    private IMongoCollection<CountryStatisticsAggregate> GetCollection()
        => this.context.Database.GetCollection<CountryStatisticsAggregate>(CollectionNames.CountryStatisticsAggregates);
}