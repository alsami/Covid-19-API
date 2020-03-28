using System;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Controllers.Presentation
{
    public class ClosedCaseStatsDto
    {
        public int Total { get; }

        public int Recovered { get; }

        public int Deaths { get; }

        public DateTime FetchedAt { get; }

        public ClosedCaseStatsDto(int total, int recovered, int deaths, DateTime fetchedAt)
        {
            this.Total = total;
            this.Recovered = recovered;
            this.Deaths = deaths;
            this.FetchedAt = fetchedAt;
        }
    }
}