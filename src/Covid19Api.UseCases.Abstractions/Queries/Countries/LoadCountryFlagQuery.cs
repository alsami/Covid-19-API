using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.Countries;

public record LoadCountryFlagQuery(string CountryCode) : IRequest<ImageDto>, ICacheableRequest
{
    public CacheConfiguration GetCacheConfiguration()
    {
        return new CacheConfiguration($"country_flag{CountryCode.ToLowerInvariant()}", TimeSpan.FromDays(1));
    }
}