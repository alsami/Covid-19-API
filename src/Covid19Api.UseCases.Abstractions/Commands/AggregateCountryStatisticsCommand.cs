using System;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Commands
{
    public class AggregateCountryStatisticsCommand : IRequest
    {
        public AggregateCountryStatisticsCommand(string[] countries, int month, int year)
        {
            this.Countries = countries ?? throw new ArgumentNullException(nameof(countries));
            this.Month = month;
            this.Year = year;
        }

        public string[] Countries { get; }

        public int Month { get; }

        public int Year { get; }
    }
}