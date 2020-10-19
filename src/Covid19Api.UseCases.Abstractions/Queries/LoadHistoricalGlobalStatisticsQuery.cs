using System;
using System.Collections.Generic;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries
{
    public class LoadHistoricalGlobalStatisticsQuery : ICacheableRequest, IRequest<IEnumerable<GlobalStatisticsDto>>
    {
        public LoadHistoricalGlobalStatisticsQuery(DateTime minFetchedAt)
        {
            this.MinFetchedAt = minFetchedAt;
        }

        public DateTime MinFetchedAt { get; }
        public CacheConfiguration GetCacheConfiguration() => new CacheConfiguration(nameof(LoadHistoricalGlobalStatisticsQuery), TimeSpan.FromMinutes(30));
    }
}