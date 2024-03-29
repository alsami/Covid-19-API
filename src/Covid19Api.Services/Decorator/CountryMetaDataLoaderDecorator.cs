using System.Text;
using System.Text.Json;
using Covid19Api.Services.Abstractions.Compression;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.Services.Abstractions.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Covid19Api.Services.Decorator;

public class CountryMetaDataLoaderDecorator : ICountryMetaDataLoader, IDisposable
{
    private const string CacheKey = "CountryMetaData";

    private readonly SemaphoreSlim mutex = new(1);

    private readonly IDistributedCache distributedCache;
    private readonly ICountryMetaDataLoader countryMetaDataLoader;
    private readonly ICompressionService compressionService;

    public CountryMetaDataLoaderDecorator(IDistributedCache distributedCache,
        ICountryMetaDataLoader countryMetaDataLoader, ICompressionService compressionService)
    {
        this.distributedCache = distributedCache;
        this.countryMetaDataLoader = countryMetaDataLoader;
        this.compressionService = compressionService;
    }


    public void Dispose()
    {
        this.mutex.Dispose();
        GC.SuppressFinalize(this);
    }

    public async Task<CountryMetaData[]> LoadCountryMetaDataAsync()
    {
        try
        {
            await this.mutex.WaitAsync();
            return await this.LoadCountryMetaDataInternalAsync();
        }
        finally
        {
            this.mutex.Release(1);
        }
    }

    private async Task<CountryMetaData[]> LoadCountryMetaDataInternalAsync()
    {
        var cached = await this.distributedCache.GetAsync(CacheKey);

        if (!(cached is null) && !cached.SequenceEqual(Array.Empty<byte>()))
        {
            var decompressed = await this.compressionService.DecompressAsync(cached);
            return JsonSerializer.Deserialize<CountryMetaData[]>(decompressed) ?? Array.Empty<CountryMetaData>();
        }

        var fetchedCountryMetaData = await this.countryMetaDataLoader.LoadCountryMetaDataAsync();
        await this.CacheAsync(fetchedCountryMetaData);

        return fetchedCountryMetaData;
    }

    private async ValueTask CacheAsync(CountryMetaData[] fetchedCountryMetaData)
    {
        var serialized = JsonSerializer.Serialize(fetchedCountryMetaData);
        var compressed = await this.compressionService.CompressAsync(Encoding.UTF8.GetBytes(serialized));

        await this.distributedCache.SetAsync(CacheKey, compressed, new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTime.UtcNow.AddDays(1)
        });
    }
}