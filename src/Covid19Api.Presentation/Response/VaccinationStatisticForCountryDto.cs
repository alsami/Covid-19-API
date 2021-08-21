using System;

namespace Covid19Api.Presentation.Response
{
    public record VaccinationStatisticForCountryDto(
        string Country, 
        string CountryCode,
        DateTime LoggedAt, 
        ulong TotalVaccinations, 
        ulong PeopleVaccinated,
        ulong PeopleFullyVaccinated, 
        double PeopleFullyVaccinatedPerHundred, 
        ulong DailyVaccinations,
        double TotalVaccinationsPerHundred, 
        double PeopleVaccinatedPerHundred, ulong DailyVaccinationsPerMillion);
}