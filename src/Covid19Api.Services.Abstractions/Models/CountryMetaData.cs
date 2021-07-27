namespace Covid19Api.Services.Abstractions.Models
{
    // ReSharper disable once SuggestBaseTypeForParameter
    public sealed record CountryMetaData(string Name, string Alpha2Code, string Alpha3Code, string[] AltSpellings);
}