using System.Collections.Generic;
using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries
{
    public class LoadHistoricalStatisticsForCountryQuery : IRequest<IEnumerable<CountryStatisticsDto>>
    {
        public LoadHistoricalStatisticsForCountryQuery(string country)
        {
            this.Country = country;
        }

        public string Country { get; }
    }
}