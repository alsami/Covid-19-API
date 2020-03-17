using System;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Controllers.Presentation
{
    public class GlobalStatsDto
    {
        public int Total { get; }

        public int Recovered { get; }
        
        public decimal RecoveredPercentage { get; }

        public int Deaths { get; }
        
        public decimal DeathsPercentage { get; }
        
        public DateTime FetchedAt { get; }

        public GlobalStatsDto(int total, int recovered, int deaths, DateTime fetchedAt)
        {
            this.Total = total;
            this.Recovered = recovered;
            this.Deaths = deaths;
            this.FetchedAt = fetchedAt;
            this.RecoveredPercentage = (decimal) this.Recovered / this.Total * 100;
            this.DeathsPercentage = (decimal) this.Deaths / this.Total * 100;
        }
    }
}