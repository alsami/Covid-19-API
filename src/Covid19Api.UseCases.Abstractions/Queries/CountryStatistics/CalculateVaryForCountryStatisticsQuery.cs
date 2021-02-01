using System;
using System.Collections.Generic;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.CountryStatistics
{
    public record CalculateVaryForCountryStatisticsQuery(string Country, DateTime MinFetchedAt) : ICacheableRequest, IRequest<IEnumerable<CountryVaryStatisticContainerDto>>
    {
        public CacheConfiguration GetCacheConfiguration() => new(nameof(CalculateVaryForCountryStatisticsQuery), TimeSpan.FromMinutes(30));
    }
}