using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Queries.GlobalStatistics;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Api.Endpoints.Rest.V1;

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
    public Task<GlobalStatisticDto> LoadGlobalAsync() => this.mediator.Send(new LoadLatestGlobalStatisticsQuery());

    [HttpGet("history")]
    public Task<IEnumerable<GlobalStatisticDto>> LoadGlobalHistorical()
    {
        var query = new LoadHistoricalGlobalStatisticsQuery(DateTime.UtcNow.Date.AddDays(-9));

        return this.mediator.Send(query);
    }
}