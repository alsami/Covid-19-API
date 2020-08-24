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
    public class CountryStatisticsController : ControllerBase
    {
        private readonly IMediator mediator;

        public CountryStatisticsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<IEnumerable<CountryStatisticsDto>> LoadLatestAsync() =>
            this.mediator.Send(new LoadLatestCountriesStatisticsQuery());

        [HttpGet("history")]
        public Task<IEnumerable<CountryStatisticsDto>> LoadHistoryAsync()
            => this.mediator.Send(new LoadHistoricalCountriesStatisticsQuery(DateTime.UtcNow.Date.AddDays(-9)));

        [HttpGet("{country}")]
        public Task<CountryStatisticsDto> LoadLatestForCountryAsync(string country) =>
            this.mediator.Send(new LoadLatestStatisticsForCountryQuery(country));

        [HttpGet("{country}/history")]
        public Task<IEnumerable<CountryStatisticsDto>> LoadHistoryForCountryAsync(string country) =>
            this.mediator.Send(new LoadHistoricalStatisticsForCountryQuery(country));
    }
}