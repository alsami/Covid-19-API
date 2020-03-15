using System;
using System.Security.Cryptography;
using System.Text;

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
            this.Id = this.Generate();
        }

        private Guid Generate()
        {
            using var hasher = MD5.Create();

            var unhashed =
                $"{this.TotalCases}{this.NewCases}{this.TotalDeaths}{this.NewDeaths}{this.RecoveredCases}{this.ActiveCases}{this.SeriousCases}";

            var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(unhashed));

            return new Guid(hashed);
        }
    }
}