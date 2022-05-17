

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Covid19Api.Domain;

public record VaccinationStatisticValue(
    DateTime LoggedAt,
    ulong TotalVaccinations,
    ulong PeopleVaccinated,
    ulong PeopleFullyVaccinated,
    double PeopleFullyVaccinatedPerHundred,
    ulong DailyVaccinations,
    double TotalVaccinationsPerHundred,
    double PeopleVaccinatedPerHundred,
    uint DailyVaccinationsPerMillion);
