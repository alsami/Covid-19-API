using System.Security.Cryptography;
using System.Text;

namespace Covid19Api.Domain;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedAutoPropertyAccessor.Global
public record GlobalStatisticsAggregate(int Total, int Recovered, int Deaths, int Month, int Year)
{
    public Guid Id => this.Generate();
    private Guid Generate()
    {
        using var hasher = MD5.Create();

        var unhashed = $"{nameof(GlobalStatisticsAggregate)}_{this.Month}.{this.Year}";

        var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(unhashed));

        return new Guid(hashed);
    }
}