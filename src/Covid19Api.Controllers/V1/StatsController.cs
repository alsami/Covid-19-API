using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Controllers.Presentation;
using Covid19Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/stats")]
    public class StatsController : ControllerBase
    {
        private readonly LatestStatsRepository latestStatsRepository;
        private readonly IMapper mapper;

        public StatsController(LatestStatsRepository latestStatsRepository, IMapper mapper)
        {
            this.latestStatsRepository = latestStatsRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<LatestStatsDto> LoadLatestAsync()
        {
            var last = await this.latestStatsRepository.MostRecentAsync();

            return this.mapper.Map<LatestStatsDto>(last);
        }
        
        [HttpGet("history")]
        public async Task<IEnumerable<LatestStatsDto>> LoadLatestHistorical()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-7);

            var latestActiveCaseStats = await this.latestStatsRepository.HistoricalAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<LatestStatsDto>>(latestActiveCaseStats);
        }
    }
}