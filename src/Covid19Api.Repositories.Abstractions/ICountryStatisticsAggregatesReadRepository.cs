using Covid19Api.Domain;

namespace Covid19Api.Repositories.Abstractions;

public interface ICountryStatisticsAggregatesReadRepository
{
    Task<CountryStatisticsAggregate?> FindAsync(string country, int month, int year);

    Task<IList<CountryStatisticsAggregate>> FindForCountryInYearAsync(string country, int year);
}