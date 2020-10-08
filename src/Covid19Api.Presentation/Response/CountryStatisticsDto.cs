using System;

namespace Covid19Api.Presentation.Response
{
    public class CountryStatisticsDto
    {
        public string Country { get; }

        public string CountryCode { get; set; }

        public int TotalCases { get; }

        public int NewCases { get; }

        public int TotalDeaths { get; }

        public int NewDeaths { get; }

        public int RecoveredCases { get; }

        public int ActiveCases { get; }

        public int SeriousCases { get; }

        public DateTime FetchedAt { get; }

        public CountryStatisticsDto(string country, string countryCode, int totalCases, int newCases, int totalDeaths,
            int newDeaths,
            int recoveredCases, int activeCases, int seriousCases, DateTime fetchedAt)
        {
            this.Country = country;
            this.CountryCode = countryCode;
            this.TotalCases = totalCases;
            this.NewCases = newCases;
            this.TotalDeaths = totalDeaths;
            this.NewDeaths = newDeaths;
            this.RecoveredCases = recoveredCases;
            this.ActiveCases = activeCases;
            this.SeriousCases = seriousCases;
            this.FetchedAt = fetchedAt;
        }
    }
}