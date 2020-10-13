using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Covid19Api.Services.Abstractions.Compression;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.Services.Abstractions.Models;
using Microsoft.Extensions.Caching.Distributed;

namespace Covid19Api.Services.Decorator
{
    public class CountryMetaDataLoaderDecorator : ICountryMetaDataLoader
    {
        private const string CacheKey = "CountryMetaData";

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

        public async Task<CountryMetaData[]> LoadCountryMetaDataByCountryAsync()
        {
            var cached = await this.distributedCache.GetAsync(CacheKey);

            if (!(cached is null) && !cached.SequenceEqual(Array.Empty<byte>()))
            {
                var decompressed = await this.compressionService.DecompressAsync(cached);
                return JsonSerializer.Deserialize<CountryMetaData[]>(decompressed);
            }

            var fetchedCountryMetaData = await this.countryMetaDataLoader.LoadCountryMetaDataByCountryAsync();

            var serialized = JsonSerializer.Serialize(fetchedCountryMetaData);
            var compressed = await this.compressionService.CompressAsync(Encoding.UTF8.GetBytes(serialized));

            await this.distributedCache.SetAsync(CacheKey, compressed, new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddDays(10)
            });

            return fetchedCountryMetaData;
        }
    }
}