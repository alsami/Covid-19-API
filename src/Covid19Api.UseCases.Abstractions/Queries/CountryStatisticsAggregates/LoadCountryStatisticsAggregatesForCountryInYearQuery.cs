using System.Collections.Generic;
using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.CountryStatisticsAggregates
{
    public sealed record LoadCountryStatisticsAggregatesForCountryInYearQuery
        (string Country, int Year) : IRequest<IEnumerable<CountryStatisticAggregateDto>>;
}