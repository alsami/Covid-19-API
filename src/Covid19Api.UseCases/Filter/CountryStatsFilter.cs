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
                    stats.Country != "Ré;union" &&
                    stats.Country != "Eswatini" &&
                    stats.Country != "CAR" &&
                    stats.Country != "Channel Islands" &&
                    stats.Country != "Faeroe Islands" &&
                    stats.Country != "Caribbean Netherlands" &&
                    stats.Country != "British Virgin Islands" &&
                    stats.Country != "St. Barth" &&
                    stats.Country != "St. Vincent Grenadines" &&
                    stats.Country != "Saint Pierre Miquelon" &&
                    !stats.Country.StartsWith("<nobr>") &&
                    !stats.Empty());
    }
}