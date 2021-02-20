using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Domain;

namespace Covid19Api.Repositories.Abstractions
{
    public interface ICountryStatisticsReadRepository
    {
        Task<IEnumerable<CountryStatistic>> HistoricalAsync(DateTime minFetchedAt);
        Task<IEnumerable<CountryStatistic>> HistoricalAsync(DateTime minFetchedAt, string country);
        Task<CountryStatistic?> FindInRangeAsync(string country, DateTime inclusiveStart,
            DateTime exclusiveEnd);

        Task<CountryStatistic> LoadCurrentAsync(string country);

        Task<IEnumerable<CountryStatistic>> LoadCurrentAsync();
    }
}