using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Covid19Api.Services.Abstractions.Caching
{
    public interface IHtmlDocumentCache
    {
        Task<HtmlDocument> LoadAsync();
    }
}