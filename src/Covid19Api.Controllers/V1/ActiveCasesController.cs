using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Controllers.Presentation;
using Covid19Api.Domain;
using Covid19Api.Repositories;
using Covid19Api.Services.Cache;
using Covid19Api.Services.Parser;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/stats/activecases")]
    public class ActiveCasesController : ControllerBase
    {
        private readonly ActiveCasesStatsRepository activeCasesStatsRepository;
        private readonly HtmlDocumentCache htmlDocumentCache;
        private readonly IMapper mapper;

        public ActiveCasesController(IMapper mapper, ActiveCasesStatsRepository activeCasesStatsRepository,
            HtmlDocumentCache htmlDocumentCache)
        {
            this.mapper = mapper;
            this.activeCasesStatsRepository = activeCasesStatsRepository;
            this.htmlDocumentCache = htmlDocumentCache;
        }

        [HttpGet]
        public async Task<ActiveCaseStatsDto> LoadLatestActiveAsync()
        {
            var fetchedAt = DateTime.UtcNow;

            var document = await this.htmlDocumentCache.LoadAsync();

            var active = ActiveCasesParser.Parse(document, fetchedAt);

            return this.mapper.Map<ActiveCaseStatsDto>(active);
        }

        [HttpGet("history")]
        public async Task<IEnumerable<ActiveCaseStatsDto>> LoadActiveCasesHistoryAsync()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-6);

            var latestActiveCaseStats = await this.activeCasesStatsRepository.HistoricalAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<ActiveCaseStatsDto>>(latestActiveCaseStats);
        }

        [HttpGet("dayhistory")]
        public async Task<IEnumerable<ActiveCaseStatsDto>> LoadActiveCasesDayHistoryAsync()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-6);

            var latestActiveCaseStats = await this.activeCasesStatsRepository.HistoricalForDayAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<ActiveCaseStatsDto>>(latestActiveCaseStats);
        }
    }
}