using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Controllers.Presentation;
using Covid19Api.Repositories;
using Covid19Api.Services.Cache;
using Covid19Api.Services.Filter;
using Covid19Api.Services.Parser;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Api.Controllers.V1
{
    [ApiController]
    [Route("api/v1/stats/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly CountryStatsRepository countryStatsRepository;
        private readonly HtmlDocumentCache htmlDocumentCache;

        public CountriesController(IMapper mapper, CountryStatsRepository countryStatsRepository,
            HtmlDocumentCache htmlDocumentCache)
        {
            this.mapper = mapper;
            this.countryStatsRepository = countryStatsRepository;
            this.htmlDocumentCache = htmlDocumentCache;
        }

        [HttpGet]
        public async Task<IEnumerable<CountryStatsDto>> LoadLatestAsync()
        {
            var fetchedAt = DateTime.UtcNow;

            var document = await this.htmlDocumentCache.LoadAsync();

            var countries = CountryStatsParser.Parse(document, fetchedAt);

            return this.mapper.Map<IEnumerable<CountryStatsDto>>(countries.Where(CountryStatsFilter.ValidOnly.Value)
                .OrderByDescending(country => country.TotalCases));
        }

        [HttpGet("history")]
        public async Task<IEnumerable<CountryStatsDto>> LoadHistoryAsync()
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-9);

            var countryHistories = await this.countryStatsRepository.HistoricalAsync(minFetchedAt);

            return this.mapper.Map<IEnumerable<CountryStatsDto>>(countryHistories);
        }

        [HttpGet("{country}")]
        public async Task<CountryStatsDto> LoadLatestForCountryAsync(string country)
        {
            var fetchedAt = DateTime.UtcNow;

            var document = await this.htmlDocumentCache.LoadAsync();

            var countries = CountryStatsParser.Parse(document, fetchedAt);

            var wanted = countries
                .Where(CountryStatsFilter.ValidOnly.Value)
                .FirstOrDefault(stats =>
                    string.Equals(stats.Country, country, StringComparison.InvariantCultureIgnoreCase));

            return this.mapper.Map<CountryStatsDto>(wanted);
        }

        [HttpGet("{country}/history")]
        public async Task<IEnumerable<CountryStatsDto>> LoadHistoryForCountryAsync(string country)
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-6);

            var statsForCountry = await this.countryStatsRepository.HistoricalAsync(minFetchedAt, country);

            return this.mapper.Map<IEnumerable<CountryStatsDto>>(statsForCountry);
        }

        [HttpGet("{country}/dayhistory")]
        public async Task<IEnumerable<CountryStatsDto>> LoadDayHistoryForCountryAsync(string country)
        {
            var minFetchedAt = DateTime.UtcNow.Date.AddDays(-6);

            var statsForCountry = await this.countryStatsRepository.HistoricalForDayAsync(minFetchedAt, country);

            return this.mapper.Map<IEnumerable<CountryStatsDto>>(statsForCountry);
        }
    }
}