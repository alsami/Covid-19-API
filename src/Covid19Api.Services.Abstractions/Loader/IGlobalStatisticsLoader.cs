using System;
using System.Threading.Tasks;
using Covid19Api.Domain;

namespace Covid19Api.Services.Abstractions.Loader
{
    public interface IGlobalStatisticsLoader
    {
        Task<GlobalStatistics> ParseAsync(DateTime fetchedAt);
    }
}