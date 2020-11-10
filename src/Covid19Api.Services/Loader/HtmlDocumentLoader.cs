using System.Net.Http;
using System.Threading.Tasks;
using Covid19Api.Constants;
using Covid19Api.Services.Abstractions.Loader;
using HtmlAgilityPack;

namespace Covid19Api.Services.Loader
{
    public class HtmlDocumentLoader : IHtmlDocumentLoader
    {
        private readonly IHttpClientFactory httpClientFactory;

        public HtmlDocumentLoader(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<HtmlDocument> LoadAsync()
        {
            var client = this.httpClientFactory.CreateClient();

            var response = await client.GetAsync(Urls.Covid19WorldometerUrl);

            var document = new HtmlDocument();

            document.LoadHtml(await response.Content.ReadAsStringAsync());

            return document;
        }
    }
}