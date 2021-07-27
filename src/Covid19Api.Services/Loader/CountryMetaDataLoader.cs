using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Covid19Api.Constants;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.Services.Abstractions.Models;
using Microsoft.Extensions.Logging;

namespace Covid19Api.Services.Loader
{
    public class CountryMetaDataLoader : ICountryMetaDataLoader
    {
        private static readonly JsonSerializerOptions SerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        private readonly ILogger<CountryMetaDataLoader> logger;
        private readonly IHttpClientFactory httpClientFactory;

        public CountryMetaDataLoader(ILogger<CountryMetaDataLoader> logger, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<CountryMetaData[]> LoadCountryMetaDataAsync()
        {
            var client = this.httpClientFactory.CreateClient();

            var response = await client.GetAsync(Urls.RestCountriesApiUrl);

            if (response.IsSuccessStatusCode)
            {
                var deserialized = await JsonSerializer.DeserializeAsync<CountryMetaData[]>(
                    await response.Content.ReadAsStreamAsync(), SerializerOptions);

                return deserialized ?? Array.Empty<CountryMetaData>();
            }

            var error = await response.Content.ReadAsStringAsync();
            this.logger.LogError("Failed load country meta-data from {url}! Status-Code: {statusCode} Error:\n{error}",
                Urls.RestCountriesApiUrl, response.StatusCode, error);

            return Array.Empty<CountryMetaData>();
        }
    }
}