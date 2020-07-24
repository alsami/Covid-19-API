using Covid19Api.Presentation.Response;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries
{
    public class LoadLatestStatisticsForCountryQuery : IRequest<CountryStatsDto>
    {
        public LoadLatestStatisticsForCountryQuery(string country)
        {
            this.Country = country;
        }

        public string Country { get; }
    }
}