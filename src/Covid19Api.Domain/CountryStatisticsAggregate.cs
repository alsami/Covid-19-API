using System;
using System.Security.Cryptography;
using System.Text;
using Covid19Api.Domain.Constants;

namespace Covid19Api.Domain
{
    // ReSharper disable UnusedAutoPropertyAccessor.Local
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public class CountryStatisticsAggregate
    {
        public Guid Id { get; private set; }

        public string Country { get; private set; }

        public string? CountryCode { get; private set; }

        public int Total { get; private set; }

        public int New { get; private set; }

        public int Deaths { get; private set; }

        public int NewDeaths { get; private set; }

        public int Recovered { get; private set; }

        public int Active { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }
        
        // ReSharper disable once UnusedMember.Global
        // This needs to be present for mongo-db!
        public string Key { get; private set; } = EntityKeys.CountryStatisticsAggregates;


        public CountryStatisticsAggregate(string country, string? countryCode, int total, int @new, int deaths,
            int newDeaths,
            int recovered, int active, int month, int year)
        {
            this.Country = country;
            this.CountryCode = countryCode;
            this.Total = total;
            this.New = @new;
            this.Deaths = deaths;
            this.NewDeaths = newDeaths;
            this.Recovered = recovered;
            this.Active = active;
            this.Month = month;
            this.Year = year;
            this.Id = this.Generate();
        }

        private Guid Generate()
        {
            using var hasher = MD5.Create();

            var valueToHash = $"{nameof(CountryStatisticsAggregate)}_{this.Country}_{this.Month}_{this.Year}";

            var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(valueToHash));

            return new Guid(hashed);
        }
    }
}