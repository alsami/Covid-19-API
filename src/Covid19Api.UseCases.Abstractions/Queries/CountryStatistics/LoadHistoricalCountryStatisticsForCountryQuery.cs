using System;
using System.Collections.Generic;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.CountryStatistics
{
    public sealed record LoadHistoricalCountryStatisticsForCountryQuery(string Country) : ICacheableRequest,
        IRequest<IEnumerable<CountryStatisticDto>>
    {
        public CacheConfiguration GetCacheConfiguration() =>
            new CacheConfiguration($"{nameof(LoadHistoricalCountryStatisticsForCountryQuery)}_{this.Country}",
                TimeSpan.FromMinutes(30));
    }
}