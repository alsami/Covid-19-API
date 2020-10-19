using System;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries
{
    public class LoadLatestStatisticsForCountryQuery : ICacheableRequest, IRequest<CountryStatisticsDto>
    {
        public LoadLatestStatisticsForCountryQuery(string country)
        {
            this.Country = country;
        }

        public string Country { get; }

        public CacheConfiguration GetCacheConfiguration() =>
            new CacheConfiguration($"{nameof(LoadLatestStatisticsForCountryQuery)}_{Country}",
                TimeSpan.FromMinutes(30));
    }
}