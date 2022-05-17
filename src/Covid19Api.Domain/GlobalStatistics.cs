using System.Security.Cryptography;
using System.Text;

namespace Covid19Api.Domain;

public record GlobalStatistics(int Total, int Recovered, int Deaths, DateTime FetchedAt)
{
    public Guid Id => this.Generate();

    private Guid Generate()
    {
        using var hasher = MD5.Create();

        var unhashed = $"GlobalStats_{this.FetchedAt.Date}";

        var hashed = hasher.ComputeHash(Encoding.UTF8.GetBytes(unhashed));

        return new Guid(hashed);
    }
}