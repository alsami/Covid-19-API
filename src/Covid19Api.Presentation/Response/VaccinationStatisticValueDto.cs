namespace Covid19Api.Presentation.Response;

public record VaccinationStatisticValueDto(DateTime LoggedAt, ulong TotalVaccinations, ulong PeopleVaccinated, ulong PeopleFullyVaccinated, double PeopleFullyVaccinatedPerHundred, ulong DailyVaccinations, double TotalVaccinationsPerHundred, double PeopleVaccinatedPerHundred, uint DailyVaccinationsPerMillion)
{
}