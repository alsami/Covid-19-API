using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Covid19Api.Services.Abstractions.Loader;
using HtmlAgilityPack;
using Microsoft.Extensions.Caching.Distributed;

namespace Covid19Api.Services.Decorator
{
    public class HtmlDocumentLoaderDecorator : IHtmlDocumentLoader
    {
        private const string Key = "HTMLDOCCACHE";

        private readonly IDistributedCache distributedCache;
        private readonly IHtmlDocumentLoader htmlDocumentLoader;

        public HtmlDocumentLoaderDecorator(IDistributedCache distributedCache, IHtmlDocumentLoader htmlDocumentLoader)
        {
            this.distributedCache = distributedCache;
            this.htmlDocumentLoader = htmlDocumentLoader;
        }

        public async Task<HtmlDocument> LoadAsync()
        {
            HtmlDocument document;
            var cached = await this.distributedCache.GetAsync(Key);

            if (!(cached is null) && !cached.SequenceEqual(Array.Empty<byte>()))
            {
                document = new HtmlDocument();
                document.LoadHtml(Encoding.UTF8.GetString(cached));
                return document;
            }

            document = await this.htmlDocumentLoader.LoadAsync();
            await this.CacheAsync(document);

            return document;
        }

        private async Task CacheAsync(HtmlDocument document)
        {
            await this.distributedCache.SetAsync(Key, Encoding.UTF8.GetBytes(document.Text),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(30)
                });
        }
    }
}