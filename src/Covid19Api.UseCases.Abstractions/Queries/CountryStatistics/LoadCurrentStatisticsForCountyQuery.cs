using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.CountryStatistics;

public sealed record LoadCurrentStatisticsForCountyQuery : ICacheableRequest, IRequest<IEnumerable<CountryStatisticDto>>
{
    public CacheConfiguration GetCacheConfiguration() =>
        new CacheConfiguration(nameof(LoadCurrentStatisticsForCountyQuery), TimeSpan.FromMinutes(30));
}