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

        public string Name { get; }

        public string Alpha2Code { get; }

        public string[] AltSpellings { get; }
    }
}