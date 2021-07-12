using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Mongo;
using Covid19Api.Repositories.Abstractions;
using MongoDB.Driver;

namespace Covid19Api.Repositories
{
    public class VaccinationStatisticWriteRepository : IVaccinationStatisticWriteRepository
    {
        private readonly Covid19ApiDbContext context;

        public VaccinationStatisticWriteRepository(Covid19ApiDbContext context)
        {
            this.context = context;
        }

        public async Task StoreAsync(IEnumerable<VaccinationStatistic> vaccinationStatistics)
        {
            var collection = this.GetCollection();
            await collection.DeleteManyAsync(_ => true);
            await collection.InsertManyAsync(vaccinationStatistics);
        }
        
        private IMongoCollection<VaccinationStatistic> GetCollection()
            => this.context.Database.GetCollection<VaccinationStatistic>(CollectionNames.VaccinationStatistic);
    }
}