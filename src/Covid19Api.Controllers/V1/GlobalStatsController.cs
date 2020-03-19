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
    public class GlobalStatsController : ControllerBase
    {
        private readonly GlobalStatsRepository globalStatsRepository;
        private readonly IMapper mapper;

        public GlobalStatsController(GlobalStatsRepository globalStatsRepository, IMapper mapper)
        {
            this.globalStatsRepository = globalStatsRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<GlobalStatsDto> LoadGlobalAsync()
        {
            var last = await this.globalStatsRepository.MostRecentAsync();

            return this.mapper.Map<GlobalStatsDto>(last);
        }
        
        [HttpGet("history")]
        public async Task<IEnumerable<GlobalStatsDto>> LoadGlobalHistorical()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-7);

            var latestActiveCaseStats = await this.globalStatsRepository.HistoricalAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<GlobalStatsDto>>(latestActiveCaseStats);
        }
        
        [HttpGet("dayhistory")]
        public async Task<IEnumerable<GlobalStatsDto>> LoadGlobalDayHistorical()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-7);

            var latestActiveCaseStats = await this.globalStatsRepository.HistoricalForDayAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<GlobalStatsDto>>(latestActiveCaseStats);
        }
    }
}