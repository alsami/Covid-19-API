using System;

namespace Covid19Api.Services.Models
{
    public class ActiveCases
    {
        public int Total { get; }

        public int Mild { get; }

        public int Serious { get; }
        
        public DateTime FetchedAt { get; }

        public ActiveCases(int total, int mild, int serious, DateTime fetchedAt)
        {
            this.Total = total;
            this.Mild = mild;
            this.Serious = serious;
            this.FetchedAt = fetchedAt;
        }
    }
}