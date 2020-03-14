using System;

namespace Covid19Api.Presentation
{
    public class ActiveCaseStatsDto
    {
        public int Total { get; }

        public int Mild { get; }

        public int Serious { get; }
        
        public DateTime FetchedAt { get; }

        public decimal MildPercentage { get; }

        public decimal SeriousPercentage { get; }

        public ActiveCaseStatsDto(int total, int mild, int serious, DateTime fetchedAt)
        {
            this.Total = total;
            this.Mild = mild;
            this.Serious = serious;
            this.FetchedAt = fetchedAt;

            this.MildPercentage = (decimal) this.Mild / this.Total * 100;
            this.SeriousPercentage = (decimal) this.Serious / this.Total * 100;
        }
    }
}