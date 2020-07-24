using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly IMediator mediator;

        public CountriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<IEnumerable<CountryStatsDto>> LoadLatestAsync() =>
            this.mediator.Send(new LoadLatestCountriesStatisticsQuery());

        [HttpGet("history")]
        public Task<IEnumerable<CountryStatsDto>> LoadHistoryAsync()
            => this.mediator.Send(new LoadHistoricalCountriesStatisticsQuery(DateTime.UtcNow.Date.AddDays(-9)));

        [HttpGet("{country}")]
        public Task<CountryStatsDto> LoadLatestForCountryAsync(string country) =>
            this.mediator.Send(new LoadLatestStatisticsForCountryQuery(country));

        [HttpGet("{country}/history")]
        public Task<IEnumerable<CountryStatsDto>> LoadHistoryForCountryAsync(string country) =>
            this.mediator.Send(new LoadHistoricalStatisticsForCountryQuery(country));
    }
}