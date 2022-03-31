using System.Globalization;
using Covid19Api.Domain;
using Covid19Api.Services.Abstractions.Loader;
using HtmlAgilityPack;

namespace Covid19Api.Services.Loader;

public class GlobalStatisticsLoader : IGlobalStatisticsLoader
{
    private readonly IHtmlDocumentLoader htmlDocumentLoader;

    public GlobalStatisticsLoader(IHtmlDocumentLoader htmlDocumentLoader)
    {
        this.htmlDocumentLoader = htmlDocumentLoader;
    }

    public async Task<GlobalStatistics> ParseAsync(DateTime fetchedAt)
    {
        var document = await this.htmlDocumentLoader.LoadAsync();

        var mainCounterNodes = document
            .DocumentNode
            .Descendants()
            .Where(node => node.HasClass("maincounter-number"))
            .ToArray();

        // total, recovered, deaths
        return new GlobalStatistics(GetIntegerValue(mainCounterNodes[0]),
            GetIntegerValue(mainCounterNodes[2]),
            GetIntegerValue(mainCounterNodes[1]), fetchedAt);
    }

    private static int GetIntegerValue(HtmlNode htmlNode)
    {
        var cleanedText = htmlNode.InnerText
            .Replace("\n", "")
            .Replace(",", "")
            .Trim();

        return int.Parse(cleanedText, NumberStyles.Any);
    }
}