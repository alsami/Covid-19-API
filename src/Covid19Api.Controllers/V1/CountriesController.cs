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
    [Route("api/v1/stats/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly CountryStatsRepository countryStatsRepository;

        public CountriesController(IMapper mapper, CountryStatsRepository countryStatsRepository)
        {
            this.mapper = mapper;
            this.countryStatsRepository = countryStatsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<CountryStatsDto>> LoadLatestAsync()
        {
            var latestCountryStats = await this.countryStatsRepository.MostRecentAsync();

            return this.mapper.Map<IEnumerable<CountryStatsDto>>(latestCountryStats);
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
            var statsForCountry = await this.countryStatsRepository.MostRecentAsync(country);

            return this.mapper.Map<CountryStatsDto>(statsForCountry);
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