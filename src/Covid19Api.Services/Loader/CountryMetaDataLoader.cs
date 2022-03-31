using System.Text.Json;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.Services.Abstractions.Models;
using Covid19Api.Services.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Covid19Api.Services.Loader;

public class CountryMetaDataLoader : ICountryMetaDataLoader
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private readonly ILogger<CountryMetaDataLoader> logger;
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IOptions<CountryLayerConfiguration> countryLayerConfiguration;

    public CountryMetaDataLoader(ILogger<CountryMetaDataLoader> logger, IHttpClientFactory httpClientFactory, IOptions<CountryLayerConfiguration> countryLayerConfiguration)
    {
        this.logger = logger;
        this.httpClientFactory = httpClientFactory;
        this.countryLayerConfiguration = countryLayerConfiguration;
    }

    public async Task<CountryMetaData[]> LoadCountryMetaDataAsync()
    {
        var client = this.httpClientFactory.CreateClient();
        var url = this.countryLayerConfiguration.Value.GetRequestUrl();
        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var deserialized = await JsonSerializer.DeserializeAsync<CountryMetaData[]>(
                await response.Content.ReadAsStreamAsync(), SerializerOptions);

            return deserialized ?? Array.Empty<CountryMetaData>();
        }

        var error = await response.Content.ReadAsStringAsync();
        this.logger.LogError("Failed load country meta-data from {Url}! Status-Code: {StatusCode} Error:\n{Error}", url, response.StatusCode, error);

        return Array.Empty<CountryMetaData>();
    }
}