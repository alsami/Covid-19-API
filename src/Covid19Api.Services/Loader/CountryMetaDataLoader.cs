using System;
using System.Net.Http;
using System.Threading.Tasks;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.Services.Abstractions.Models;
using Microsoft.Extensions.Logging;

namespace Covid19Api.Services.Loader
{
    public class CountryMetaDataLoader : ICountryMetaDataLoader
    {
#pragma warning disable S1075
        private const string ApiUrl = "https://restcountries.eu/rest/v2/all";
#pragma warning restore

        private readonly ILogger<CountryMetaDataLoader> logger;
        private readonly IHttpClientFactory httpClientFactory;

        public CountryMetaDataLoader(ILogger<CountryMetaDataLoader> logger, IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<CountryMetaData[]> LoadCountryMetaDataByCountryAsync()
        {
            var client = this.httpClientFactory.CreateClient();

            var response = await client.GetAsync(ApiUrl);

            if (response.IsSuccessStatusCode) return await response.Content.ReadAsAsync<CountryMetaData[]>();

            var error = await response.Content.ReadAsStringAsync();
            this.logger.LogError("Failed load country meta-data from {url}! Status-Code: {statusCode} Error:\n{error}",
                ApiUrl, response.StatusCode, error);

            return Array.Empty<CountryMetaData>();
        }
    }
}