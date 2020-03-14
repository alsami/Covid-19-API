using System;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Domain
{
    public class LatestStats
    {
        public Guid Id { get; private set; }
        
        public int Total { get; private set; }

        public int Recovered { get; private set; }

        public int Deaths { get; private set; }
        
        public DateTime FetchedAt { get; private set; }

        public LatestStats(Guid id, int total, int recovered, int deaths, DateTime fetchedAt)
        {
            this.Id = id;
            this.Total = total;
            this.Recovered = recovered;
            this.Deaths = deaths;
            this.FetchedAt = fetchedAt;
        }
    }
}