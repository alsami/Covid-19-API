using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Queries.GlobalStatistics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/global")]
    public class GlobalStatisticsController : ControllerBase
    {
        private readonly IMediator mediator;

        public GlobalStatisticsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public Task<GlobalStatisticsDto> LoadGlobalAsync() => this.mediator.Send(new LoadLatestGlobalStatisticsQuery());

        [HttpGet("history")]
        public Task<IEnumerable<GlobalStatisticsDto>> LoadGlobalHistorical()
        {
            var command = new LoadHistoricalGlobalStatisticsQuery(DateTime.UtcNow.Date.AddDays(-9));

            return this.mediator.Send(command);
        }
    }
}