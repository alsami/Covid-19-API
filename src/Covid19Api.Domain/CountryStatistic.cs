using System.Security.Cryptography;
using System.Text;

// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace Covid19Api.Domain;

public record CountryStatistic(string Country, string? CountryCode, int TotalCases, int NewCases, int TotalDeaths,
    int NewDeaths,
    int RecoveredCases, int ActiveCases, DateTime FetchedAt)
{

    public Guid Id => this.Generate();

    public bool Empty()
    {
        return this.TotalCases == 0 && this.NewCases == 0 &&
               this.TotalDeaths == 0 && this.NewDeaths == 0 &&
               this.RecoveredCases == 0 && this.ActiveCases == 0;
    }

    private Guid Generate()
    {
        using var hasher = MD5.Create();

        var valueToHash =
            $"{this.Country}{this.FetchedAt.Date:O}";

        var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(valueToHash));

        return new Guid(hashed);
    }
}