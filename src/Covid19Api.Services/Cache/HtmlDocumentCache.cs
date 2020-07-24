using System;
using System.Net.Http;
using System.Threading.Tasks;
using Covid19Api.Constants;
using Covid19Api.Services.Abstractions.Caching;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;

namespace Covid19Api.Services.Cache
{
    public class HtmlDocumentCache : IHtmlDocumentCache
    {
        private readonly IMemoryCache memoryCache;
        private readonly IHttpClientFactory httpClientFactory;
        private const string Key = "HTMLDOCCACHE";

        public HtmlDocumentCache(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            this.memoryCache = memoryCache;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<HtmlDocument> LoadAsync()
        {
            var cachedTask = this.memoryCache.Get<Task<HttpResponseMessage>>(Key);

            if (cachedTask != null)
            {
                var cached = await cachedTask;

                if (cached.IsSuccessStatusCode)
                {
                    var document = new HtmlDocument();

                    document.LoadHtml(await cached.Content.ReadAsStringAsync());

                    return document;
                }
            }

            var client = this.httpClientFactory.CreateClient();

            var responseTask = client.GetAsync(Urls.CovidInfoWorldOmetersUrl);

            await this.memoryCache.Set(Key, responseTask, new MemoryCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(30)
            });

            var loaded = await responseTask;

            var loadedDocument = new HtmlDocument();

            loadedDocument.LoadHtml(await loaded.Content.ReadAsStringAsync());

            return loadedDocument;
        }
    }
}