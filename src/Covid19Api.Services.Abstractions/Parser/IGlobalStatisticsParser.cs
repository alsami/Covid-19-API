using System;
using System.Threading.Tasks;
using Covid19Api.Domain;

namespace Covid19Api.Services.Abstractions.Parser
{
    public interface IGlobalStatisticsParser
    {
        Task<GlobalStatistics> ParseAsync(DateTime fetchedAt);
    }
}