namespace Covid19Api.Services.Configuration
{
    public class VaccinationStatisticsConfiguration
    {
        public string Url { get; set; } =
            "https://raw.githubusercontent.com/owid/covid-19-data/master/public/data/vaccinations/vaccinations.json";
    }
}