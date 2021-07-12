using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.UseCases.Abstractions.Queries.Vaccinations;
using MediatR;

namespace Covid19Api.UseCases.Queries.Vaccinations
{
    public class LoadVaccinationStatisticsForCountryQueryHandler : IRequestHandler<LoadVaccinationStatisticsForCountryQuery, VaccinationStatisticDto>
    {
        private readonly IVaccinationStatisticReadRepository vaccinationStatisticReadRepository;
        private readonly IMapper mapper;
        
        public LoadVaccinationStatisticsForCountryQueryHandler(IVaccinationStatisticReadRepository vaccinationStatisticReadRepository, IMapper mapper)
        {
            this.vaccinationStatisticReadRepository = vaccinationStatisticReadRepository;
            this.mapper = mapper;
        }

        public async Task<VaccinationStatisticDto> Handle(LoadVaccinationStatisticsForCountryQuery request, CancellationToken cancellationToken)
        {
            var vaccinationStatistics = await this.vaccinationStatisticReadRepository.LoadCurrentAsync(request.CountryOrCountryCode);

            return this.mapper.Map<VaccinationStatisticDto>(vaccinationStatistics);
        }
    }
}