using System;
using System.Collections.Generic;
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
    [Route("api/v1/stats")]
    public class GlobalStatsController : ControllerBase
    {
        private readonly GlobalStatsRepository globalStatsRepository;
        private readonly IMapper mapper;
        private readonly HtmlDocumentCache htmlDocumentCache;

        public GlobalStatsController(GlobalStatsRepository globalStatsRepository, IMapper mapper,
            HtmlDocumentCache htmlDocumentCache)
        {
            this.globalStatsRepository = globalStatsRepository;
            this.mapper = mapper;
            this.htmlDocumentCache = htmlDocumentCache;
        }

        [HttpGet]
        public async Task<GlobalStatsDto> LoadGlobalAsync()
        {
            var fetchedAt = DateTime.UtcNow;

            var document = await this.htmlDocumentCache.LoadAsync();

            var latest = GlobalStatsParser.Parse(document, fetchedAt);

            return this.mapper.Map<GlobalStatsDto>(latest);
        }

        [HttpGet("history")]
        public async Task<IEnumerable<GlobalStatsDto>> LoadGlobalHistorical()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-9);

            var latestActiveCaseStats = await this.globalStatsRepository.HistoricalAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<GlobalStatsDto>>(latestActiveCaseStats);
        }
    }
}