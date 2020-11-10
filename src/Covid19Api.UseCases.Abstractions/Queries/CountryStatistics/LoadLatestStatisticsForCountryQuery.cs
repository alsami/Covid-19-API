using System;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.CountryStatistics
{
    public sealed record LoadLatestStatisticsForCountryQuery(string Country) : ICacheableRequest, IRequest<CountryStatisticDto>
    {
        public CacheConfiguration GetCacheConfiguration() =>
            new CacheConfiguration($"{nameof(LoadLatestStatisticsForCountryQuery)}_{this.Country}",
                TimeSpan.FromMinutes(30));
    }
}