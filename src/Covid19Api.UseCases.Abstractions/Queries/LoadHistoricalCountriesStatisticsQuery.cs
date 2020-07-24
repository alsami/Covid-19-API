using System;
using System.Collections.Generic;
using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries
{
    public class LoadHistoricalCountriesStatisticsQuery : IRequest<IEnumerable<CountryStatsDto>>
    {
        public LoadHistoricalCountriesStatisticsQuery(DateTime minFetchedAt)
        {
            this.MinFetchedAt = minFetchedAt;
        }

        public DateTime MinFetchedAt { get; }
    }
}