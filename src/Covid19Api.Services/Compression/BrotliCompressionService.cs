using System.IO.Compression;
using Covid19Api.Services.Abstractions.Compression;

namespace Covid19Api.Services.Compression;

public class BrotliCompressionService : ICompressionService
{
    public async ValueTask<byte[]> CompressAsync(byte[] uncompressed, CancellationToken cancellationToken = default)
    {
        await using var inputStream = new MemoryStream(uncompressed);
        await using var outputStream = new MemoryStream();

        await using (var compressionStream = new BrotliStream(outputStream, CompressionLevel.Fastest))
            await inputStream.CopyToAsync(compressionStream, cancellationToken);

        var compressed = outputStream.ToArray();

        return compressed;
    }

    public async ValueTask<byte[]> DecompressAsync(byte[] compressed, CancellationToken cancellationToken = default)
    {
        await using var inputMemoryStream = new MemoryStream(compressed);
        await using var outputMemoryStream = new MemoryStream();

        await using (var compressionStream = new BrotliStream(inputMemoryStream, CompressionMode.Decompress))
            await compressionStream.CopyToAsync(outputMemoryStream, cancellationToken);

        return outputMemoryStream.ToArray();
    }
}