using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Presentation.Response
{
    public class GlobalStatsDto
    {
        public int Total { get; }

        public int Recovered { get; }

        public int Deaths { get; }

        public int Active { get; }

        public DateTime FetchedAt { get; }

        public GlobalStatsDto(int total, int recovered, int deaths, DateTime fetchedAt)
        {
            this.Total = total;
            this.Recovered = recovered;
            this.Deaths = deaths;
            this.Active = this.Total - (this.Recovered + this.Deaths);
            this.FetchedAt = fetchedAt;
        }
    }
}