using System;
using System.Collections.Generic;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries
{
    public class LoadHistoricalStatisticsForCountryQuery : ICacheableRequest,
        IRequest<IEnumerable<CountryStatisticsDto>>
    {
        public LoadHistoricalStatisticsForCountryQuery(string country)
        {
            this.Country = country;
        }

        public string Country { get; }

        public CacheConfiguration GetCacheConfiguration() =>
            new CacheConfiguration($"{nameof(LoadHistoricalStatisticsForCountryQuery)}_{Country}",
                TimeSpan.FromMinutes(30));
    }
}