using System;
using System.Threading.Tasks;
using Covid19Api.Presentation;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Api.Controllers
{
    [ApiController]
    [Route("api/v1/stats")]
    public class StatsController : ControllerBase
    {
        [HttpGet("latest")]
        public Task<LatestStatsDto> Load() => Task.FromResult(new LatestStatsDto(0, 0, 0, DateTime.UtcNow));
    }
}