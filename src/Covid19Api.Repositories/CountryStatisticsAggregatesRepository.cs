using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using MongoDB.Driver;

namespace Covid19Api.Repositories
{
    public class CountryStatisticsAggregatesRepository : ICountryStatisticsAggregatesRepository
    {
        private readonly Covid19ApiDbContext context;

        public CountryStatisticsAggregatesRepository(Covid19ApiDbContext context)
        {
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

        private IMongoCollection<CountryStatisticsAggregate> GetCollection()
            => this.context.Database.GetCollection<CountryStatisticsAggregate>(CollectionNames
                .CountryStatisticsAggregates);
    }
}