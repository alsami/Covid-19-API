using System;
using System.Security.Cryptography;
using System.Text;
using Covid19Api.Domain.Constants;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Domain
{
    public class GlobalStatistics
    {
        public Guid Id { get; private set; }

        public int Total { get; private set; }

        public int Recovered { get; private set; }

        public int Deaths { get; private set; }

        public DateTime FetchedAt { get; private set; }

        // ReSharper disable once UnusedMember.Global
        // This needs to be present for mongo-db!
        public string Key { get; private set; } = EntityKeys.GlobalStatistics;

        public GlobalStatistics(int total, int recovered, int deaths, DateTime fetchedAt)
        {
            this.Total = total;
            this.Recovered = recovered;
            this.Deaths = deaths;
            this.FetchedAt = fetchedAt;
            this.Id = this.Generate();
        }

        private Guid Generate()
        {
            using var hasher = MD5.Create();

            var unhashed = $"GlobalStats_{this.FetchedAt.Date}";

            var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(unhashed));

            return new Guid(hashed);
        }
    }
}