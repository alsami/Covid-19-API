using System;
using System.Collections.Generic;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.CountryStatistics
{
    public class LoadLatestCountriesStatisticsQuery : ICacheableRequest, IRequest<IEnumerable<CountryStatisticDto>>
    {
        public CacheConfiguration GetCacheConfiguration() =>
            new CacheConfiguration(nameof(LoadLatestCountriesStatisticsQuery), TimeSpan.FromMinutes(30));
    }
}