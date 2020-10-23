using System.Collections.Generic;
using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.GlobalStatisticsAggregates
{
    public class LoadGlobalStatisticsAggregatesForYearQuery : IRequest<IEnumerable<GlobalStatisticsAggregateDto>>
    {
        public LoadGlobalStatisticsAggregatesForYearQuery(int year)
        {
            this.Year = year;
        }

        public int Year { get; }
    }
}