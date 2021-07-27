using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Domain;

namespace Covid19Api.Repositories.Abstractions
{
    public interface IVaccinationStatisticReadRepository
    {
        Task<VaccinationStatistic> LoadCurrentAsync(string countryOrCountryCode);

        Task<IEnumerable<VaccinationStatistic>> LoadLatestFourCountriesAsync();
    }
}