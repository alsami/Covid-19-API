using System;
using System.Security.Cryptography;
using System.Text;

namespace Covid19Api.Domain
{
    // ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
    // ReSharper disable UnusedAutoPropertyAccessor.Global
    public class GlobalStatisticsAggregate
    {
        public GlobalStatisticsAggregate(int total, int recovered, int deaths, int month, int year)
        {
            this.Total = total;
            this.Recovered = recovered;
            this.Deaths = deaths;
            this.Month = month;
            this.Year = year;
            this.Id = this.Generate();
        }

        public Guid Id { get; private set; }
        public int Total { get; private set; }
        public int Recovered { get; private set; }
        public int Deaths { get; private set; }
        public int Month { get; private set; }
        public int Year { get; private set; }

        private Guid Generate()
        {
            using var hasher = MD5.Create();

            var unhashed = $"{nameof(GlobalStatisticsAggregate)}_{this.Month}.{this.Year}";

            var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(unhashed));

            return new Guid(hashed);
        }
    }
}