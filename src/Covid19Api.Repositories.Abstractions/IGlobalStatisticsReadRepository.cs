using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Domain;

namespace Covid19Api.Repositories.Abstractions
{
    public interface IGlobalStatisticsReadRepository
    {
        Task<GlobalStatistics> LoadCurrentAsync();
        
        Task<IEnumerable<GlobalStatistics>> HistoricalAsync(DateTime minFetchedAt);
        Task<GlobalStatistics?> FindInRangeAsync(DateTime inclusiveStart, DateTime inclusiveEnd);
    }
}