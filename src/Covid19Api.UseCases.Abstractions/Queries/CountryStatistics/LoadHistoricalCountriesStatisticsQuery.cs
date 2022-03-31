using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.CountryStatistics;

public sealed record LoadHistoricalCountriesStatisticsQuery(DateTime MinFetchedAt) : ICacheableRequest, IRequest<IEnumerable<CountryStatisticDto>>
{
    public CacheConfiguration GetCacheConfiguration()
        => new CacheConfiguration(nameof(LoadHistoricalCountriesStatisticsQuery), TimeSpan.FromMinutes(30));
}