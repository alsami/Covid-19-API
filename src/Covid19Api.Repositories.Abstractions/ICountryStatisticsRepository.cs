using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Domain;

namespace Covid19Api.Repositories.Abstractions
{
    public interface ICountryStatisticsRepository
    {
        Task<CountryStatistics> MostRecentAsync(string country);
        Task<IEnumerable<CountryStatistics>> HistoricalAsync(DateTime minFetchedAt);
        Task<IEnumerable<CountryStatistics>> HistoricalAsync(DateTime minFetchedAt, string country);
        Task<IEnumerable<CountryStatistics>> HistoricalForDayAsync(DateTime minFetchedAt, string country);

        Task<CountryStatistics?> FindInRangeAsync(string country, DateTime inclusiveStart,
            DateTime exclusiveEnd);

        Task StoreManyAsync(IEnumerable<CountryStatistics> countryStats);
    }
}