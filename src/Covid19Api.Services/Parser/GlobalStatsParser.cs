using System;
using System.Globalization;
using System.Linq;
using Covid19Api.Domain;
using HtmlAgilityPack;

namespace Covid19Api.Services.Parser
{
    public static class GlobalStatsParser
    {
        public static GlobalStats Parse(HtmlDocument document, DateTime fetchedAt)
        {
            var mainCounterNodes = document
                .DocumentNode
                .Descendants()
                .Where(node => node.HasClass("maincounter-number"))
                .ToArray();

            // total, recovered, deaths
            return new GlobalStats(GetIntegerValue(mainCounterNodes[0]),
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