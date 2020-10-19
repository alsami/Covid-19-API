using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries
{
    public class LoadGlobalStatisticsAggregate : IRequest<GlobalStatisticsAggregateDto?>
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