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
    }
}