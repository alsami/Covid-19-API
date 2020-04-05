using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Memory;

namespace Covid19Api.Services.Cache
{
    public class HtmlDocumentCache
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
                
                var document = new HtmlDocument();

                document.LoadHtml(await cached.Content.ReadAsStringAsync());

                return document;
            };

            var client = this.httpClientFactory.CreateClient();
            
            var response = client.GetAsync("https://worldometers.info/coronavirus");

            await this.memoryCache.Set(Key, response);
            
            var loadedDocument = new HtmlDocument();

            var loaded = await response;

            loadedDocument.LoadHtml(await loaded.Content.ReadAsStringAsync());

            return loadedDocument;
        }
    }
}