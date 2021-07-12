using System.Threading.Tasks;
using Covid19Api.Presentation.Response;
using Covid19Api.UseCases.Abstractions.Queries.Vaccinations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Covid19Api.Endpoints.Rest.V1
{
    [ApiController]
    [Route("api/v1/vaccination-statistics")]
    public class VaccinationStatisticsController : ControllerBase
    {
        private readonly IMediator mediator;

        public VaccinationStatisticsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("{countryOrCountryCode}")]
        public Task<VaccinationStatisticDto> LoadVaccinationStatisticsForCountryAsync(string countryOrCountryCode)
        {
            var query = new LoadVaccinationStatisticsForCountryQuery(countryOrCountryCode);
            return this.mediator.Send(query);
        }
    }
}