using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Queries.CountryStatisticsAggregates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/countries/{country}/aggregates")]
    public class CountryStatisticsCountryAggregatesController : ControllerBase
    {
        [HttpGet("{year:int}")]
        public Task<IEnumerable<CountryStatisticsAggregateDto>> LoadAggregatesForCountryInYearAsync(
            [FromServices] IMediator mediator, string country, int year)
        {
            var query = new LoadCountryStatisticsAggregatesForCountryInYearQuery(country, year);
            return mediator.Send(query);
        }
    }
}