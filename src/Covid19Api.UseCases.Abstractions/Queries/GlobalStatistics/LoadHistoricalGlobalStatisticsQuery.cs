using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.GlobalStatistics;

public sealed record LoadHistoricalGlobalStatisticsQuery(DateTime MinFetchedAt) : ICacheableRequest, IRequest<IEnumerable<GlobalStatisticDto>>
{
    public CacheConfiguration GetCacheConfiguration() =>
        new CacheConfiguration(nameof(LoadHistoricalGlobalStatisticsQuery), TimeSpan.FromMinutes(30));
}