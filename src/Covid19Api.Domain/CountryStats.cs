using System;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Domain
{
    public class CountryStats
    {
        public Guid Id { get; private set; }
        
        public string Country { get; private set; }

        public int TotalCases { get; private set; }

        public int NewCases { get; private set; }

        public int TotalDeaths { get; private set; }

        public int NewDeaths { get; private set; }

        public int RecoveredCases { get; private set; }

        public int ActiveCases { get; private set; }

        public int SeriousCases { get; private set; }

        public DateTime FetchedAt { get; private set; }

        public CountryStats(Guid id, string country, int totalCases, int newCases, int totalDeaths, int newDeaths,
            int recoveredCases, int activeCases, int seriousCases, DateTime fetchedAt)
        {
            this.Id = id;
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