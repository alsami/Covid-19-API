using System;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Presentation.Response
{
    public class ActiveCaseStatsDto
    {
        public int Total { get; }

        public int Mild { get; }

        public int Serious { get; }

        public DateTime FetchedAt { get; }

        public ActiveCaseStatsDto(int total, int mild, int serious, DateTime fetchedAt)
        {
            this.Total = total;
            this.Mild = mild;
            this.Serious = serious;
            this.FetchedAt = fetchedAt;
        }
    }
}