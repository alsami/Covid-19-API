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
    [Route("api/v1/stats/closedcases")]
    public class ClosedCasesController : ControllerBase
    {
        private readonly ClosedCasesRepository closedCasesRepository;
        private readonly IMapper mapper;

        public ClosedCasesController(IMapper mapper,
            ClosedCasesRepository closedCasesRepository)
        {
            this.mapper = mapper;
            this.closedCasesRepository = closedCasesRepository;
        }

        [HttpGet]
        public async Task<ClosedCaseStatsDto> LoadLatestInactiveAsync()
        {
            var last = await this.closedCasesRepository.MostRecentAsync();

            return this.mapper.Map<ClosedCaseStatsDto>(last);
        }

        [HttpGet("history")]
        public async Task<IEnumerable<ClosedCaseStatsDto>> LoadClosedCasesHistoryAsync()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-7);

            var latestActiveCaseStats = await this.closedCasesRepository.HistoricalAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<ClosedCaseStatsDto>>(latestActiveCaseStats);
        }
    }
}