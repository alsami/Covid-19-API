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
    [Route("api/v1/cases")]
    public class CasesController : ControllerBase
    {
        private readonly ActiveCasesStatsRepository activeCasesStatsRepository;
        private readonly ClosedCasesRepository closedCasesRepository;
        private readonly IMapper mapper;

        public CasesController(IMapper mapper, ActiveCasesStatsRepository activeCasesStatsRepository,
            ClosedCasesRepository closedCasesRepository)
        {
            this.mapper = mapper;
            this.activeCasesStatsRepository = activeCasesStatsRepository;
            this.closedCasesRepository = closedCasesRepository;
        }

        [HttpGet("active")]
        public async Task<ActiveCaseStatsDto> LoadLatestActiveAsync()
        {
            var latestActiveCaseStats = await this.activeCasesStatsRepository.MostRecentAsync();

            return this.mapper.Map<ActiveCaseStatsDto>(latestActiveCaseStats);
        }

        [HttpGet("active/history")]
        public async Task<IEnumerable<ActiveCaseStatsDto>> LoadActiveCasesHistoryAsync()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-7);

            var latestActiveCaseStats = await this.activeCasesStatsRepository.HistoricalAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<ActiveCaseStatsDto>>(latestActiveCaseStats);
        }

        [HttpGet("closed")]
        public async Task<ClosedCaseStatsDto> LoadLatestInactiveAsync()
        {
            var last = await this.closedCasesRepository.MostRecentAsync();

            return this.mapper.Map<ClosedCaseStatsDto>(last);
        }
        
        [HttpGet("closed/history")]
        public async Task<IEnumerable<ClosedCaseStatsDto>> LoadClosedCasesHistoryAsync()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-7);

            var latestActiveCaseStats = await this.closedCasesRepository.HistoricalAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<ClosedCaseStatsDto>>(latestActiveCaseStats);
        }
    }
}