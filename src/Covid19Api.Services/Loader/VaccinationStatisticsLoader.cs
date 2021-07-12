using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.Services.Configuration;
using Covid19Api.Services.Models;
using Microsoft.Extensions.Options;

namespace Covid19Api.Services.Loader
{
    public class VaccinationStatisticsLoader : IVaccinationStatisticsLoader
    {
        private readonly IOptions<VaccinationStatisticsConfiguration> vaccinationStatisticsConfiguration;
        private readonly IHttpClientFactory httpClientFactory;

        public VaccinationStatisticsLoader(IOptions<VaccinationStatisticsConfiguration> vaccinationStatisticsConfiguration, IHttpClientFactory httpClientFactory)
        {
            this.vaccinationStatisticsConfiguration = vaccinationStatisticsConfiguration;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<VaccinationStatistic[]> LoadAsync()
        {
            var client = this.httpClientFactory.CreateClient();

            var response = await client.GetAsync(this.vaccinationStatisticsConfiguration.Value.Url);
            var models = JsonSerializer.Deserialize<VaccinationStatisticModel[]>(await response.Content.ReadAsStringAsync())!;

            return models
                .Where(statistic => statistic.Country != "Asia" && statistic.Country != "World")
                .Select(statistic =>
                {
                    var validValues = statistic.Values.Where(value => value.TotalVaccinations.HasValue);
                    return new VaccinationStatistic(statistic.Country, statistic.CountryCode,
                        // ReSharper disable once ConvertClosureToMethodGroup
                        validValues.Select(statisticValue => CreateVaccinationStatisticValue(statisticValue)).ToArray());
                })
                .ToArray();
        }

        private static VaccinationStatisticValue CreateVaccinationStatisticValue(VaccinationStatisticValueModel value)
        {
            return new(
                value.LoggedAt, 
                value.TotalVaccinations!.Value, 
                value.PeopleVaccinated, 
                value.PeopleFullyVaccinated, 
                value.PeopleFullyVaccinatedPerHundred, 
                value.DailyVaccinations, 
                value.TotalVaccinationsPerHundred, 
                value.PeopleVaccinatedPerHundred, 
                value.DailyVaccinationsPerMillion);
        }
    }
}