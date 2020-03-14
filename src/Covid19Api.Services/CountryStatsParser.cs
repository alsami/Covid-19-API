using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Covid19Api.Services.Models;
using HtmlAgilityPack;

namespace Covid19Api.Services
{
    public static class CountryStatsParser
    {
        public static IEnumerable<CountryStats> Parse(HtmlDocument document, DateTime fetchedAt)
        {
            var tableRows = GetTableRows(document);

            foreach (var tableRow in tableRows)
                yield return Parse(tableRow, fetchedAt);
        }

        private static IEnumerable<HtmlNode> GetTableRows(HtmlDocument document)
            => document
                .DocumentNode
                .Descendants()
                .First(node => node.Id == "main_table_countries")
                .ChildNodes
                .First(node => node.Name == "tbody")
                .ChildNodes
                .Where(node => node.GetType() != typeof(HtmlTextNode))
                .ToArray();

        private static IEnumerable<HtmlNode> GetTableData(HtmlNode htmlNode)
            => htmlNode
                .ChildNodes
                .Where(node => node.Name == "td")
                .ToArray();

        private static CountryStats Parse(HtmlNode htmlNode, DateTime fetchedAt)
        {
            var tableDataNodes = GetTableData(htmlNode).ToArray();
            
            var country = ParseCountry(tableDataNodes[0]);
            var totalCases = ParseIntegerValue(tableDataNodes[1]);
            var newCases = ParseIntegerValue(tableDataNodes[2]);
            var totalDeaths = ParseIntegerValue(tableDataNodes[3]);
            var newDeaths = ParseIntegerValue(tableDataNodes[4]);
            var recovered = ParseIntegerValue(tableDataNodes[5]);
            var active = ParseIntegerValue(tableDataNodes[6]);
            var serious = ParseIntegerValue(tableDataNodes[7]);
            
            return new CountryStats(country, totalCases, newCases, totalDeaths, newDeaths, recovered, active, serious, fetchedAt);
        }

        private static string ParseCountry(HtmlNode htmlNode)
        {
            var anchorNode = htmlNode.ChildNodes.FirstOrDefault(node => node.Name == "a");

            if (anchorNode is null)
            {
                var regularNode = htmlNode.InnerHtml.Trim().Replace("&ccedil;", "ç");

                return regularNode.Contains("<span", StringComparison.InvariantCulture)
                    ? null
                    : regularNode;
            }

            var anchorValue = anchorNode.InnerHtml.Trim().Replace("&ccedil;", "ç");

            return anchorValue.Contains("<span", StringComparison.InvariantCulture) ? null : anchorValue;
        }

        private static int ParseIntegerValue(HtmlNode htmlNode)
        {
            var value = htmlNode.InnerHtml;

            return string.IsNullOrWhiteSpace(value) ? 0 : int.Parse(htmlNode.InnerHtml, NumberStyles.Any);
        }
    }
}