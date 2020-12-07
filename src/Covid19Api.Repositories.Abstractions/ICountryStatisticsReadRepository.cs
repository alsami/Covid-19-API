using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Domain;

namespace Covid19Api.Repositories.Abstractions
{
    public interface ICountryStatisticsReadRepository
    {
        Task<CountryStatistic> MostRecentAsync(string country);
        Task<IEnumerable<CountryStatistic>> HistoricalAsync(DateTime minFetchedAt);
        Task<IEnumerable<CountryStatistic>> HistoricalAsync(DateTime minFetchedAt, string country);
        Task<IEnumerable<CountryStatistic>> HistoricalForDayAsync(DateTime minFetchedAt, string country);

        Task<CountryStatistic?> FindInRangeAsync(string country, DateTime inclusiveStart,
            DateTime exclusiveEnd);
    }
}