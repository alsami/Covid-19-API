using System;
using System.Globalization;
using System.Linq;
using Covid19Api.Domain;
using HtmlAgilityPack;

namespace Covid19Api.Services.Parser
{
    public static class LatestStatsParser
    {
        public static LatestStats Parse(HtmlDocument document, DateTime fetchedAt)
        {
            var mainCounterNodes = document
                .DocumentNode
                .Descendants()
                .Where(node => node.HasClass("maincounter-number"))
                .ToArray();

            // total, recovered, deaths
            return new LatestStats(Guid.NewGuid(), GetIntegerValue(mainCounterNodes[0]),
                GetIntegerValue(mainCounterNodes[2]),
                GetIntegerValue(mainCounterNodes[1]), fetchedAt);
        }

        private static int GetIntegerValue(HtmlNode htmlNode) =>
            int.Parse(htmlNode.ChildNodes.First(node => node.Name == "span").InnerHtml, NumberStyles.Any);
    }
}