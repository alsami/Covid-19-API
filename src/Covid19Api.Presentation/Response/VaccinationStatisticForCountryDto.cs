using System;

namespace Covid19Api.Presentation.Response
{
    public record VaccinationStatisticForCountryDto(
        string Country, 
        string CountryCode,
        DateTime LoggedAt, 
        uint TotalVaccinations, 
        uint PeopleVaccinated,
        uint PeopleFullyVaccinated, 
        double PeopleFullyVaccinatedPerHundred, 
        uint DailyVaccinations,
        double TotalVaccinationsPerHundred, 
        double PeopleVaccinatedPerHundred, uint DailyVaccinationsPerMillion);
}