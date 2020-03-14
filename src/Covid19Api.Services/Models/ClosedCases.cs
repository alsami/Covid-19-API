using System;

namespace Covid19Api.Services.Models
{
    public class ClosedCases
    {
        public int Total { get; }

        public int Recovered { get; }

        public int Deaths { get; }

        public DateTime FetchedAt { get; }

        public ClosedCases(int total, int recovered, int deaths, DateTime fetchedAt)
        {
            this.Total = total;
            this.Recovered = recovered;
            this.Deaths = deaths;
            this.FetchedAt = fetchedAt;
        }
    }
}