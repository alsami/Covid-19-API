using System.Collections.Generic;
using System.Threading.Tasks;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Queries.GlobalStatisticsAggregates;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/global/aggregates")]
    public class GlobalStatisticsAggregatesController
    {
        [HttpGet("{year:int}")]
        public Task<IEnumerable<GlobalStatisticsAggregateDto>> LoadForYearAsync([FromServices] IMediator mediator,
            int year)
        {
            var query = new LoadGlobalStatisticsAggregatesForYearQuery(year);
            return mediator.Send(query);
        }
    }
}