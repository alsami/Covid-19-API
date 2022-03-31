// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
namespace Covid19Api.Services.Configuration;

public class CountryLayerConfiguration
{
    public string ApiUrl { get; set; } = null!;

    public string ApiKey { get; set; } = null!;

    public string GetRequestUrl() => $"{this.ApiUrl}?access_key={this.ApiKey}";
}