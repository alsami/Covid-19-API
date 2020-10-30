using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.GlobalStatisticsAggregates
{
    public class LoadGlobalStatisticsAggregate : IRequest<GlobalStatisticAggregateDto?>
    {
        public LoadGlobalStatisticsAggregate(int month, int year)
        {
            this.Month = month;
            this.Year = year;
        }

        public int Month { get; }

        public int Year { get; }
    }
}