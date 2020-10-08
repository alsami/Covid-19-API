using System;
using Covid19Api.Domain;

namespace Covid19Api.UseCases.Filter
{
    public static class CountryStatsFilter
    {
        public static readonly Lazy<Func<CountryStatistics?, bool>> ValidOnly =
            new Lazy<Func<CountryStatistics?, bool>>(
                stats =>
                    stats != null &&
                    stats.Country != "World" &&
                    !stats.Country.StartsWith("<nobr>") &&
                    !stats.Empty());
    }
}