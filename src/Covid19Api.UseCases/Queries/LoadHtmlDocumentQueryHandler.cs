using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Covid19Api.Constants;
using Covid19Api.UseCases.Abstractions.Queries;
using HtmlAgilityPack;
using MediatR;

namespace Covid19Api.UseCases.Queries
{
    public class LoadHtmlDocumentQueryHandler : IRequestHandler<LoadHtmlDocumentQuery, HtmlDocument>
    {
        private readonly IHttpClientFactory httpClientFactory;

        public LoadHtmlDocumentQueryHandler(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<HtmlDocument> Handle(LoadHtmlDocumentQuery request, CancellationToken cancellationToken)
        {
            var client = this.httpClientFactory.CreateClient();

            var response = await client.GetAsync(Urls.CovidInfoWorldOmetersUrl, cancellationToken);

            var content = await response.Content.ReadAsStringAsync();

            var document = new HtmlDocument();

            document.LoadHtml(content);

            return document;
        }
    }
}