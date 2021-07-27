using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Covid19Api.Presentation.Response;
using Covid19Api.Repositories.Abstractions;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.UseCases.Abstractions.Queries.Vaccinations;
using MediatR;

namespace Covid19Api.UseCases.Queries.Vaccinations
{
    public class LoadVaccinationStatisticsForCountriesQueryHandler : IRequestHandler<LoadVaccinationStatisticsForCountriesQuery, IEnumerable<VaccinationStatisticForCountryDto>>
    {
        private readonly IVaccinationStatisticReadRepository vaccinationStatisticReadRepository;
        private readonly ICountryMetaDataLoader countryMetaDataLoader;

        public LoadVaccinationStatisticsForCountriesQueryHandler(IVaccinationStatisticReadRepository vaccinationStatisticReadRepository, ICountryMetaDataLoader countryMetaDataLoader)
        {
            this.vaccinationStatisticReadRepository = vaccinationStatisticReadRepository;
            this.countryMetaDataLoader = countryMetaDataLoader;
        }

        public async Task<IEnumerable<VaccinationStatisticForCountryDto>> Handle(LoadVaccinationStatisticsForCountriesQuery request, CancellationToken cancellationToken)
        {
            var statistics = await this.vaccinationStatisticReadRepository.LoadLatestFourCountriesAsync();
            var countryMetaData = await this.countryMetaDataLoader.LoadCountryMetaDataAsync();
            var countryMetaDataByCountryIsoCode = countryMetaData.ToDictionary(metaData => metaData.Alpha3Code);

            return statistics
                .Where(statistic => countryMetaDataByCountryIsoCode.ContainsKey(statistic.CountyCode))
                .Select(statistic =>
                {
                    var statisticValue = statistic.Values.First()!;
                    return new VaccinationStatisticForCountryDto(statistic.Country, 
                        countryMetaDataByCountryIsoCode[statistic.CountyCode].Alpha2Code, 
                        statisticValue.LoggedAt,
                        statisticValue.TotalVaccinations, 
                        statisticValue.PeopleVaccinated, 
                        statisticValue.PeopleFullyVaccinated, 
                        statisticValue.PeopleVaccinatedPerHundred,
                        statisticValue.DailyVaccinations,
                        statisticValue.TotalVaccinationsPerHundred, 
                        statisticValue.PeopleVaccinatedPerHundred,
                        statisticValue.DailyVaccinationsPerMillion);
                });
        }
    }
}