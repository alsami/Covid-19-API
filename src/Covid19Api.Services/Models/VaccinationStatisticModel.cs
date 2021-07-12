using System.Text.Json.Serialization;

namespace Covid19Api.Services.Models
{
    internal class VaccinationStatisticModel
    {
        [JsonPropertyName("country")] 
        public string Country { get; set; } = null!;

        [JsonPropertyName("iso_code")] 
        public string CountryCode { get; set; } = null!;

        [JsonPropertyName("data")] 
        public VaccinationStatisticValueModel[] Values { get; set; } = null!;
    }
}