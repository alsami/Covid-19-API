using System;
using System.Text.Json.Serialization;

namespace Covid19Api.Services.Models
{
    internal class VaccinationStatisticValueModel
    {
        [JsonPropertyName("date")] 
        public DateTime LoggedAt { get; set; }

        [JsonPropertyName("total_vaccinations")] 
        public uint? TotalVaccinations { get; set; }
        
        [JsonPropertyName("people_vaccinated")] 
        public uint PeopleVaccinated { get; set; }
        
        [JsonPropertyName("people_fully_vaccinated")] 
        public uint PeopleFullyVaccinated { get; set; }
        
        [JsonPropertyName("people_fully_vaccinated_per_hundred")] 
        public double PeopleFullyVaccinatedPerHundred { get; set; }

        [JsonPropertyName("daily_vaccinations")] 
        public uint DailyVaccinations { get; set; }

        [JsonPropertyName("total_vaccinations_per_hundred")] 
        public double TotalVaccinationsPerHundred { get; set; }

        [JsonPropertyName("people_vaccinated_per_hundred")] 
        public double PeopleVaccinatedPerHundred { get; set; }

        [JsonPropertyName("daily_vaccinations_per_million")] 
        public uint DailyVaccinationsPerMillion { get; set; }
    }
}