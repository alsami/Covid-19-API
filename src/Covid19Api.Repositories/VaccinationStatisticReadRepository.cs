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
    public class VaccinationStatisticReadRepository : IVaccinationStatisticReadRepository
    {
        private readonly Covid19ApiDbContext context;

        public VaccinationStatisticReadRepository(Covid19ApiDbContext context)
        {
            this.context = context;
        }

        public async Task<VaccinationStatistic> LoadCurrentAsync(string countryOrCountryCode)
        {
            var collection = this.GetCollection();
            var filter = new ExpressionFilterDefinition<VaccinationStatistic>(statistic =>
                statistic.Country.ToLower() == countryOrCountryCode.ToLower() ||
                statistic.CountyCode.ToLower() == countryOrCountryCode.ToLower());
            var cursor = await collection.FindAsync(filter);
            return await cursor.SingleAsync();
        }

        public async Task<IEnumerable<VaccinationStatistic>> LoadLatestFourCountriesAsync()
        { 
            var collection = this.GetCollection();

            var cursor = collection.Find(Builders<VaccinationStatistic>.Filter.Where(_ => true));
            var res = await cursor.ToListAsync();
            return res.Select(r =>
            {
                return new VaccinationStatistic(r.Country, r.CountyCode, new[] {r.Values.OrderByDescending(s => s.LoggedAt).First()});
            }).ToList();
        }

        private IMongoCollection<VaccinationStatistic> GetCollection()
            => this.context.Database.GetCollection<VaccinationStatistic>(CollectionNames.VaccinationStatistic);
    }
}