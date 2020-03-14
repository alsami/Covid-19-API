using System;

namespace Covid19Api.Services.Models
{
    public class CountryStats
    {
        public string Country { get; }

        public int TotalCases { get; }

        public int NewCases { get; }

        public int TotalDeaths { get; }

        public int NewDeaths { get; }

        public int RecoveredCases { get; }

        public int ActiveCases { get; }

        public int SeriousCases { get; }

        public DateTime FetchedAt { get; }

        public CountryStats(string country, int totalCases, int newCases, int totalDeaths, int newDeaths,
            int recoveredCases, int activeCases, int seriousCases, DateTime fetchedAt)
        {
            this.Country = country;
            this.TotalCases = totalCases;
            this.NewCases = newCases;
            this.TotalDeaths = totalDeaths;
            this.NewDeaths = newDeaths;
            this.RecoveredCases = recoveredCases;
            this.ActiveCases = activeCases;
            this.SeriousCases = seriousCases;
            this.FetchedAt = fetchedAt;
        }

        public override string ToString()
        {
            return
                $"Country: {this.Country}, Total: {this.TotalCases}, New: {this.NewCases}, Total-Deaths: {this.TotalDeaths}, New-Deaths: {this.NewDeaths}, Recovered: {this.RecoveredCases}, Active: {this.ActiveCases}, Serious: {this.SeriousCases}";
        }
    }
}