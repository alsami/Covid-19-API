using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.GlobalStatistics;

public sealed record LoadLatestGlobalStatisticsQuery : ICacheableRequest, IRequest<GlobalStatisticDto>
{
    public CacheConfiguration GetCacheConfiguration() =>
        new CacheConfiguration(nameof(LoadLatestGlobalStatisticsQuery), TimeSpan.FromMinutes(30));
}