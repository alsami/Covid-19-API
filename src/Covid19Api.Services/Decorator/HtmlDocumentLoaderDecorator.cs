using System.Text;
using Covid19Api.Services.Abstractions.Compression;
using Covid19Api.Services.Abstractions.Loader;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Distributed;

namespace Covid19Api.Services.Decorator;

public class HtmlDocumentLoaderDecorator : IHtmlDocumentLoader, IDisposable
{
    private const string Key = "HTMLDOCCACHE";

    private readonly SemaphoreSlim mutex = new SemaphoreSlim(1);

    private readonly IDistributedCache distributedCache;
    private readonly IHtmlDocumentLoader htmlDocumentLoader;
    private readonly ICompressionService compressionService;

    public HtmlDocumentLoaderDecorator(IDistributedCache distributedCache, IHtmlDocumentLoader htmlDocumentLoader,
        ICompressionService compressionService)
    {
        this.distributedCache = distributedCache;
        this.htmlDocumentLoader = htmlDocumentLoader;
        this.compressionService = compressionService;
    }

    public void Dispose()
    {
        this.mutex.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<HtmlDocument> LoadAsync()
    {
        try
        {
            await this.mutex.WaitAsync();
            return await this.LoadInternalAsync();
        }
        finally
        {
            this.mutex.Release(1);
        }
    }

    private async Task<HtmlDocument> LoadInternalAsync()
    {
        HtmlDocument document;
        var compressed = await this.distributedCache.GetAsync(Key);

        if (!(compressed is null) && !compressed.SequenceEqual(Array.Empty<byte>()))
        {
            var decompressed = await this.compressionService.DecompressAsync(compressed);
            document = new HtmlDocument();
            document.LoadHtml(Encoding.UTF8.GetString(decompressed));
            return document;
        }

        document = await this.htmlDocumentLoader.LoadAsync();
        await this.CacheAsync(document);

        return document;
    }

    private async Task CacheAsync(HtmlDocument document)
    {
        var compressed = await this.compressionService.CompressAsync(Encoding.UTF8.GetBytes(document.Text));
        await this.distributedCache.SetAsync(Key, compressed,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(5)
            });
    }
}