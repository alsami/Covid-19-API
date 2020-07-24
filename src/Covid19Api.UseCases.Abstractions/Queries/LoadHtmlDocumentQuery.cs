using HtmlAgilityPack;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Queries
{
    public class LoadHtmlDocumentQuery : IRequest<HtmlDocument>
    {
    }
}