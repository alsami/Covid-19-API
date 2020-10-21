using System.Threading.Tasks;
using Covid19Api.Domain;

namespace Covid19Api.Repositories.Abstractions
{
    public interface ICountryStatisticsAggregatesRepository
    {
        Task StoreAsync(CountryStatisticsAggregate countryStatisticsAggregate);

        Task<CountryStatisticsAggregate?> FindAsync(string country, int month, int year);
    }
}