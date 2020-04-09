using System;
using Covid19Api.Domain;

namespace Covid19Api.Services.Filter
{
    public static class CountryStatsFilter
    {
        public static readonly Lazy<Func<CountryStats, bool>> ValidOnly = new Lazy<Func<CountryStats, bool>>(stats =>
            !string.IsNullOrWhiteSpace(stats.Country) &&
            stats.Country != "World" &&
            !stats.Country.StartsWith("<nobr>") &&
            !stats.Empty());
    }
}