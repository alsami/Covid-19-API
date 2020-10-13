using System.Threading;
using System.Threading.Tasks;

namespace Covid19Api.Services.Abstractions.Compression
{
    public interface ICompressionService
    {
        ValueTask<byte[]> CompressAsync(byte[] uncompressed, CancellationToken cancellationToken = default);

        ValueTask<byte[]> DecompressAsync(byte[] compressed, CancellationToken cancellationToken = default);
    }
}