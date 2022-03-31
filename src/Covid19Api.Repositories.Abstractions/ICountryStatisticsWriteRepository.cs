using Covid19Api.Domain;

namespace Covid19Api.Repositories.Abstractions;

public interface ICountryStatisticsWriteRepository
{
    Task StoreManyAsync(IEnumerable<CountryStatistic> countryStatistics);
}