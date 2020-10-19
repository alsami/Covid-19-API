// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable ClassNeverInstantiated.Global

namespace Covid19Api.Services.Abstractions.Models
{
    public class CountryMetaData
    {
        public CountryMetaData(string name, string alpha2Code, string[] altSpellings)
        {
            this.Name = name;
            this.Alpha2Code = alpha2Code;
            this.AltSpellings = altSpellings;
        }

        private CountryMetaData()
        {
        }

        public string Name { get; set; } = null!;

        public string Alpha2Code { get; set; } = null!;

        public string[] AltSpellings { get; set; } = null!;
    }
}