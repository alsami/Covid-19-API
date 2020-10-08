using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Services.Abstractions.Caching;
using Covid19Api.Services.Abstractions.Parser;
using HtmlAgilityPack;

namespace Covid19Api.Services.Parser
{
    public class GlobalStatisticsParser : IGlobalStatisticsParser
    {
        private readonly IHtmlDocumentCache htmlDocumentCache;

        public GlobalStatisticsParser(IHtmlDocumentCache htmlDocumentCache)
        {
            this.htmlDocumentCache = htmlDocumentCache;
        }

        public async Task<GlobalStatistics> ParseAsync(DateTime fetchedAt)
        {
            var document = await this.htmlDocumentCache.LoadAsync();

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
}