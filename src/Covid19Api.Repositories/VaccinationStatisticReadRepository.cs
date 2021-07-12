using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using MongoDB.Driver;

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
        
        private IMongoCollection<VaccinationStatistic> GetCollection()
            => this.context.Database.GetCollection<VaccinationStatistic>(CollectionNames.VaccinationStatistic);
    }
}