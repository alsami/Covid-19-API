using System;
using System.Security.Cryptography;
using System.Text;

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

        public ActiveCaseStats(int total, int mild, int serious, DateTime fetchedAt)
        {
            this.Total = total;
            this.Mild = mild;
            this.Serious = serious;
            this.FetchedAt = fetchedAt;
            this.Id = this.Generate();
        }

        private Guid Generate()
        {
            using var hasher = MD5.Create();

            var unhashed = $"{this.Total}{this.Mild}{this.Serious}";

            var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(unhashed));
            
            return new Guid(hashed);
        }
    }
}