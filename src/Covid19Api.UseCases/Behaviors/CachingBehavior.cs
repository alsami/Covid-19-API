using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Covid19Api.Services.Abstractions.Compression;
using Covid19Api.UseCases.Abstractions.Base;
using Covid19Api.UseCases.Abstractions.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace Covid19Api.UseCases.Behaviors
{
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull where TResponse : class
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
        {
            Converters =
            {
                new JsonStringEnumConverter()
            },
            IgnoreNullValues = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = false
        };
        
        private readonly IDistributedCache distributedCache;
        private readonly ICompressionService compressionService;

        public CachingBehavior(IDistributedCache distributedCache, ICompressionService compressionService)
        {
            this.distributedCache = distributedCache;
            this.compressionService = compressionService;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!(request is ICacheableRequest cacheableRequest))
            {
                return await next();
            }

            var configuration = cacheableRequest.GetCacheConfiguration();

            var cached = await FindInCacheAsync(configuration);

            if (cached is {}) return cached;

            var response = await next();

            await this.CacheAsync(configuration, response);

            return response;
        }

        private async Task<TResponse?> FindInCacheAsync(CacheConfiguration configuration)
        {
            var compressed = await this.distributedCache.GetAsync(configuration.Key);

            if (compressed is null || compressed.SequenceEqual(Array.Empty<byte>())) return null;

            var decompressed = await this.compressionService.DecompressAsync(compressed);

            return JsonSerializer.Deserialize<TResponse>(decompressed, this.jsonSerializerOptions);
        }

        private async Task CacheAsync(CacheConfiguration configuration, TResponse response)
        {
            var serialized = JsonSerializer.Serialize(response, this.jsonSerializerOptions);

            var compressed = await this.compressionService.CompressAsync(Encoding.UTF8.GetBytes(serialized));

            await this.distributedCache.SetAsync(configuration.Key,
                compressed, new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.Add(configuration.Duration)
                });
        }
    }
}