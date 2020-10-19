using MediatR;

namespace Covid19Api.UseCases.Abstractions.Commands
{
    public class AggregateGlobalStatisticsCommand : IRequest
    {
        public AggregateGlobalStatisticsCommand(int month, int year)
        {
            this.Month = month;
            this.Year = year;
        }

        public int Month { get; }

        public int Year { get; }
    }
}