using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.CountryStatisticsAggregates
{
    public class LoadCountryStatisticsAggregate : IRequest<CountryStatisticAggregateDto?>
    {
        public LoadCountryStatisticsAggregate(string country, int month, int year)
        {
            this.Country = country;
            this.Month = month;
            this.Year = year;
        }

        public string Country { get; }
        public int Month { get; }

        public int Year { get; }
    }
}