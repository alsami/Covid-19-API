using System.Security.Cryptography;
using System.Text;

namespace Covid19Api.Domain;

public record CountryStatisticsAggregate(string Country, string? CountryCode, int Total, int New, int Deaths,
    int NewDeaths,
    int Recovered, int Active, int Month, int Year)
{
    public Guid Id => this.Generate();

    private Guid Generate()
    {
        using var hasher = MD5.Create();

        var valueToHash = $"{nameof(CountryStatisticsAggregate)}_{this.Country}_{this.Month}_{this.Year}";

        var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(valueToHash));

        return new Guid(hashed);
    }
}