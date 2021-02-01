using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Covid19Api.Services.Tests")]
namespace Covid19Api.Services.Calculators
{
    internal static class ValueKeys
    {
        public const string Total = "Total";
        public const string New = "New";
        public const string Active = "Active";
        public const string Deaths = "Deaths";
        public const string NewDeaths = "NewDeaths";
        public const string Recovered = "Recovered";
    }
}