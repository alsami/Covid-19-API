using System;

namespace Covid19Api.UseCases.Abstractions.Models
{
    public class CacheConfiguration
    {
        public CacheConfiguration(string key, TimeSpan duration)
        {
            this.Key = !string.IsNullOrWhiteSpace(key) ? key : throw new ArgumentNullException(nameof(key));
            this.Duration = duration;
        }

        public string Key { get; }

        public TimeSpan Duration { get; }
    }
}