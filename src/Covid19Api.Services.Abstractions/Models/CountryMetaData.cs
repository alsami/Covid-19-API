namespace Covid19Api.Services.Abstractions.Models
{
    public sealed record CountryMetaData(string Name, string Alpha2Code, string[] AltSpellings);
}