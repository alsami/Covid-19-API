using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Domain;

namespace Covid19Api.Repositories.Abstractions
{
    public interface IGlobalStatisticsRepository
    {
        Task StoreAsync(GlobalStatistics globalStatistics);
        Task<IEnumerable<GlobalStatistics>> HistoricalAsync(DateTime minFetchedAt);
        Task<IEnumerable<GlobalStatistics>> HistoricalForDayAsync(DateTime minFetchedAt);
        Task<GlobalStatistics?> FindInRangeAsync(DateTime inclusiveStart, DateTime inclusiveEnd);
    }
}