using System.Collections.Generic;
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

        [HttpGet]
        public Task<IEnumerable<VaccinationStatisticForCountryDto>> LoadForCountriesAsync()
        {
            var query = new LoadVaccinationStatisticsForCountriesQuery();
            return this.mediator.Send(query);
        }
    }
}