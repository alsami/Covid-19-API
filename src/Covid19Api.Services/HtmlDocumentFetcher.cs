using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Covid19Api.Services
{
    public static class HtmlDocumentFetcher
    {
        public static async Task<HtmlDocument> FetchAsync(HttpClient client)
        {
            var response = await client.GetAsync("https://worldometers.info/coronavirus");

            var content = await response.Content.ReadAsStringAsync();

            var document = new HtmlDocument();

            document.LoadHtml(content);

            return document;
        }
    }
}