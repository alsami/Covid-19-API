using System;
using System.Collections.Generic;
using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries
{
    public class LoadHistoricalGlobalStatisticsQuery : IRequest<IEnumerable<GlobalStatisticsDto>>
    {
        public LoadHistoricalGlobalStatisticsQuery(DateTime minFetchedAt)
        {
            this.MinFetchedAt = minFetchedAt;
        }

        public DateTime MinFetchedAt { get; }
    }
}