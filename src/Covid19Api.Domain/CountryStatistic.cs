using System;
using System.Security.Cryptography;
using System.Text;
using Covid19Api.Domain.Constants;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Domain
{
    public class CountryStatistic
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

        public DateTime FetchedAt { get; private set; }

        // ReSharper disable once UnusedMember.Global
        // This needs to be present for mongo-db!
        public string Key { get; private set; } = EntityKeys.CountryStatistics;

        public CountryStatistic(string country, string? countryCode, int totalCases, int newCases, int totalDeaths,
            int newDeaths,
            int recoveredCases, int activeCases, DateTime fetchedAt)
        {
            this.Country = country;
            this.CountryCode = countryCode;
            this.TotalCases = totalCases;
            this.NewCases = newCases;
            this.TotalDeaths = totalDeaths;
            this.NewDeaths = newDeaths;
            this.RecoveredCases = recoveredCases;
            this.ActiveCases = activeCases;
            this.FetchedAt = fetchedAt;
            this.Id = this.Generate();
        }

        public bool Empty()
        {
            return this.TotalCases == 0 && this.NewCases == 0 &&
                   this.TotalDeaths == 0 && this.NewDeaths == 0 &&
                   this.RecoveredCases == 0 && this.ActiveCases == 0;
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