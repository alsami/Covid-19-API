using System;

namespace Covid19Api.Presentation.Response
{
    public class GlobalStatisticsAggregateDto
    {
        public GlobalStatisticsAggregateDto(Guid id, int total, int recovered, int deaths, int month, int year)
        {
            this.Id = id;
            this.Total = total;
            this.Recovered = recovered;
            this.Deaths = deaths;
            this.Month = month;
            this.Year = year;
        }

        public Guid Id { get; set; }
        public int Total { get; set; }
        public int Recovered { get; set; }
        public int Deaths { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}