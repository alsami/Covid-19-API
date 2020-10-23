using System;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.GlobalStatistics
{
    public class LoadLatestGlobalStatisticsQuery : ICacheableRequest, IRequest<GlobalStatisticsDto>
    {
        public CacheConfiguration GetCacheConfiguration() =>
            new CacheConfiguration(nameof(LoadLatestGlobalStatisticsQuery), TimeSpan.FromMinutes(30));
    }
}