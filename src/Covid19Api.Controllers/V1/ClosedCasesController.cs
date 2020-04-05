using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Controllers.Presentation;
using Covid19Api.Repositories;
using Covid19Api.Services.Cache;
using Covid19Api.Services.Parser;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/stats/closedcases")]
    public class ClosedCasesController : ControllerBase
    {
        private readonly ClosedCasesRepository closedCasesRepository;
        private readonly IMapper mapper;
        private readonly HtmlDocumentCache htmlDocumentCache;

        public ClosedCasesController(IMapper mapper,
            ClosedCasesRepository closedCasesRepository, HtmlDocumentCache htmlDocumentCache)
        {
            this.mapper = mapper;
            this.closedCasesRepository = closedCasesRepository;
            this.htmlDocumentCache = htmlDocumentCache;
        }

        [HttpGet]
        public async Task<ClosedCaseStatsDto> LoadLatestInactiveAsync()
        {
            var fetchedAt = DateTime.UtcNow;

            var document = await this.htmlDocumentCache.LoadAsync();

            var closedCases = ClosedCasesParser.Parse(document, fetchedAt);

            return this.mapper.Map<ClosedCaseStatsDto>(closedCases);
        }

        [HttpGet("history")]
        public async Task<IEnumerable<ClosedCaseStatsDto>> LoadClosedCasesHistoryAsync()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-6);

            var closdedCasesHistory = await this.closedCasesRepository.HistoricalAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<ClosedCaseStatsDto>>(closdedCasesHistory);
        }

        [HttpGet("dayhistory")]
        public async Task<IEnumerable<ClosedCaseStatsDto>> LoadClosedCasesDayHistoryAsync()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-6);

            var closedCasesDayHistory = await this.closedCasesRepository.HistoricalForDayAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<ClosedCaseStatsDto>>(closedCasesDayHistory);
        }
    }
}