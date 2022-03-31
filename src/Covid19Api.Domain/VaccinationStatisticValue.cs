

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Covid19Api.Domain;

public class VaccinationStatisticValue
{
    public VaccinationStatisticValue(DateTime loggedAt, ulong totalVaccinations, ulong peopleVaccinated, ulong peopleFullyVaccinated, double peopleFullyVaccinatedPerHundred, ulong dailyVaccinations, double totalVaccinationsPerHundred, double peopleVaccinatedPerHundred, uint dailyVaccinationsPerMillion)
    {
        this.LoggedAt = loggedAt;
        this.TotalVaccinations = totalVaccinations;
        this.PeopleVaccinated = peopleVaccinated;
        this.PeopleFullyVaccinated = peopleFullyVaccinated;
        this.PeopleFullyVaccinatedPerHundred = peopleFullyVaccinatedPerHundred;
        this.DailyVaccinations = dailyVaccinations;
        this.TotalVaccinationsPerHundred = totalVaccinationsPerHundred;
        this.PeopleVaccinatedPerHundred = peopleVaccinatedPerHundred;
        this.DailyVaccinationsPerMillion = dailyVaccinationsPerMillion;
    }

    public DateTime LoggedAt { get; private set; }

    public ulong TotalVaccinations { get; private set; }
        
    public ulong PeopleVaccinated { get; private set; }

    public ulong PeopleFullyVaccinated { get; private set; }

    public double PeopleFullyVaccinatedPerHundred { get; private set; }

    public ulong DailyVaccinations { get; private set; }

    public double TotalVaccinationsPerHundred { get; private set; }

    public double PeopleVaccinatedPerHundred { get; private set; }

    public uint DailyVaccinationsPerMillion { get; private set; }
}