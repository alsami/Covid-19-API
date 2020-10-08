using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.Services.Abstractions.Models;
using Microsoft.Extensions.Caching.Distributed;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Covid19Api.Services.Decorator
{
    public class CountryMetaDataLoaderDecorator : ICountryMetaDataLoader
    {
        private const string CacheKey = "CountryMetaData";

        private readonly IDistributedCache distributedCache;
        
        private readonly ICountryMetaDataLoader countryMetaDataLoader;


        public CountryMetaDataLoaderDecorator(IDistributedCache distributedCache, ICountryMetaDataLoader countryMetaDataLoader)
        {
            this.distributedCache = distributedCache;
            this.countryMetaDataLoader = countryMetaDataLoader;
        }

        public async Task<CountryMetaData[]> LoadCountryMetaDataByCountryAsync()
        {
            var cached = await this.distributedCache.GetAsync(CacheKey);

            if (!(cached is null) && !cached.SequenceEqual(Array.Empty<byte>()))
            {
                return JsonSerializer.Deserialize<CountryMetaData[]>(cached);
            }

            var fetchedCountryMetaData = await this.countryMetaDataLoader.LoadCountryMetaDataByCountryAsync();

            var serialized = JsonSerializer.Serialize(fetchedCountryMetaData);

            await this.distributedCache.SetAsync(CacheKey, Encoding.UTF8.GetBytes(serialized));

            return fetchedCountryMetaData;
        }
    }
}