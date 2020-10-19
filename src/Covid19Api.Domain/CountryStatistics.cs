using System;
using System.Security.Cryptography;
using System.Text;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Domain
{
    public class CountryStatistics
    {
        public Guid Id { get; private set; }

        public string Country { get; private set; }

        public string? CountryCode { get; private set; }

        public int TotalCases { get; private set; }

        public int NewCases { get; private set; }

        public int TotalDeaths { get; private set; }

        public int NewDeaths { get; private set; }

        public int RecoveredCases { get; private set; }

        public int ActiveCases { get; private set; }

        public int SeriousCases { get; private set; }

        public DateTime FetchedAt { get; private set; }

        public CountryStatistics(string country, string? countryCode, int totalCases, int newCases, int totalDeaths,
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
            this.Id = this.Generate();
        }

        public bool Empty()
        {
            return this.TotalCases == 0 && this.NewCases == 0 &&
                   this.TotalDeaths == 0 && this.NewDeaths == 0 &&
                   this.RecoveredCases == 0 && this.ActiveCases == 0 &&
                   this.SeriousCases == 0;
        }

        private Guid Generate()
        {
            using var hasher = MD5.Create();

            var valueToHash =
                $"{this.Country}{this.FetchedAt.Date:O}";

            var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(valueToHash));

            return new Guid(hashed);
        }
    }
}