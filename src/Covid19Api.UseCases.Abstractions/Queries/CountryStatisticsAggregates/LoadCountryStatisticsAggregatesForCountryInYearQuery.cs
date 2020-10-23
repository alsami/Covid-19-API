using System;
using System.Collections.Generic;
using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries.CountryStatisticsAggregates
{
    public class
        LoadCountryStatisticsAggregatesForCountryInYearQuery : IRequest<IEnumerable<CountryStatisticsAggregateDto>>
    {
        public LoadCountryStatisticsAggregatesForCountryInYearQuery(string country, int year)
        {
            this.Country = country;
            this.Year = year;

            if (string.IsNullOrWhiteSpace(this.Country)) throw new ArgumentNullException(nameof(country));
        }

        public string Country { get; }
        public int Year { get; }
    }
}