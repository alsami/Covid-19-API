using System;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Domain
{
    public class ActiveCaseStats
    {
        public Guid Id { get; private set; }
        
        public int Total { get; private set; }

        public int Mild { get; private set; }

        public int Serious { get; private set; }
        
        public DateTime FetchedAt { get; private set; }

        public ActiveCaseStats(Guid id, int total, int mild, int serious, DateTime fetchedAt)
        {
            this.Id = id;
            this.Total = total;
            this.Mild = mild;
            this.Serious = serious;
            this.FetchedAt = fetchedAt;
        }
    }
}