using System.Collections.Generic;
using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries
{
    public class LoadLatestCountriesStatisticsQuery : IRequest<IEnumerable<CountryStatsDto>>
    {
    }
}