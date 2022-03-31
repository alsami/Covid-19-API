using HtmlAgilityPack;

namespace Covid19Api.Services.Abstractions.Loader;

public interface IHtmlDocumentLoader
{
    Task<HtmlDocument> LoadAsync();
}