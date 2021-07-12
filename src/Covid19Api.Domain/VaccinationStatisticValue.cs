using System;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Covid19Api.Domain
{
    public class VaccinationStatisticValue
    {
        public VaccinationStatisticValue(DateTime loggedAt, uint totalVaccinations, uint peopleVaccinated, uint peopleFullyVaccinated, double peopleFullyVaccinatedPerHundred, uint dailyVaccinations, double totalVaccinationsPerHundred, double peopleVaccinatedPerHundred, uint dailyVaccinationsPerMillion)
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

        public uint TotalVaccinations { get; private set; }
        
        public uint PeopleVaccinated { get; private set; }

        public uint PeopleFullyVaccinated { get; private set; }

        public double PeopleFullyVaccinatedPerHundred { get; private set; }

        public uint DailyVaccinations { get; private set; }

        public double TotalVaccinationsPerHundred { get; private set; }

        public double PeopleVaccinatedPerHundred { get; private set; }

        public uint DailyVaccinationsPerMillion { get; private set; }
    }
}