using Covid19Api.Domain;

namespace Covid19Api.Services.Abstractions.Loader;

public interface ICountryStatisticsLoader
{
    Task<IEnumerable<CountryStatistic?>> ParseAsync(DateTime fetchedAt,
        Func<CountryStatistic?, bool>? filter = null);
}