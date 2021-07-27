using System;
using System.Collections.Generic;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.Vaccinations
{
    public record LoadVaccinationStatisticsForCountriesQuery : IRequest<IEnumerable<VaccinationStatisticForCountryDto>>, ICacheableRequest
    {
        public CacheConfiguration GetCacheConfiguration() =>
            new(nameof(LoadVaccinationStatisticsForCountryQuery), TimeSpan.FromMinutes(30));
    }
}