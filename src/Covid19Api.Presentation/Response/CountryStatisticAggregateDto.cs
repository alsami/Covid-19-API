using System;

namespace Covid19Api.Presentation.Response
{
    public class CountryStatisticAggregateDto
    {
        public CountryStatisticAggregateDto(Guid id, string country, string? countryCode, int total, int @new,
            int deaths,
            int newDeaths,
            int recovered, int active, int month, int year)
        {
            this.Id = id;
            this.Country = country;
            this.CountryCode = countryCode;
            this.Total = total;
            this.New = @new;
            this.Deaths = deaths;
            this.NewDeaths = newDeaths;
            this.Recovered = recovered;
            this.Active = active;
            this.Month = month;
            this.Year = year;
        }

        public Guid Id { get; set; }
        public string Country { get; set; }
        public string? CountryCode { get; set; }
        public int Total { get; set; }
        public int New { get; set; }
        public int Deaths { get; set; }
        public int NewDeaths { get; set; }
        public int Recovered { get; set; }
        public int Active { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}