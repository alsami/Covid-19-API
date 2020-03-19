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
    [Route("api/v1/stats/activecases")]
    public class ActiveCasesController : ControllerBase
    {
        private readonly ActiveCasesStatsRepository activeCasesStatsRepository;
        private readonly IMapper mapper;

        public ActiveCasesController(IMapper mapper, ActiveCasesStatsRepository activeCasesStatsRepository)
        {
            this.mapper = mapper;
            this.activeCasesStatsRepository = activeCasesStatsRepository;
        }

        [HttpGet]
        public async Task<ActiveCaseStatsDto> LoadLatestActiveAsync()
        {
            var latestActiveCaseStats = await this.activeCasesStatsRepository.MostRecentAsync();

            return this.mapper.Map<ActiveCaseStatsDto>(latestActiveCaseStats);
        }

        [HttpGet("history")]
        public async Task<IEnumerable<ActiveCaseStatsDto>> LoadActiveCasesHistoryAsync()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-7);

            var latestActiveCaseStats = await this.activeCasesStatsRepository.HistoricalAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<ActiveCaseStatsDto>>(latestActiveCaseStats);
        }
        
        [HttpGet("dayhistory")]
        public async Task<IEnumerable<ActiveCaseStatsDto>> LoadActiveCasesDayHistoryAsync()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-7);

            var latestActiveCaseStats = await this.activeCasesStatsRepository.HistoricalForDayAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<ActiveCaseStatsDto>>(latestActiveCaseStats);
        }
    }
}