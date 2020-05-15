using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Covid19Api.Domain;
using HtmlAgilityPack;

namespace Covid19Api.Services.Parser
{
    public static class CountryStatsParser
    {
        public static IEnumerable<CountryStats> Parse(HtmlDocument document, DateTime fetchedAt)
        {
            var tableRows = GetTableRows(document);

            foreach (var tableRow in tableRows)
                yield return Parse(tableRow, fetchedAt);
        }

        private static CountryStats Parse(HtmlNode htmlNode, DateTime fetchedAt)
        {
            var tableDataNodes = GetTableDataNodes(htmlNode).ToArray();

            var country = ParseCountry(tableDataNodes[1]);
            var totalCases = ParseIntegerValue(tableDataNodes[2]);
            var newCases = ParseIntegerValue(tableDataNodes[3]);
            var totalDeaths = ParseIntegerValue(tableDataNodes[4]);
            var newDeaths = ParseIntegerValue(tableDataNodes[5]);
            var recovered = ParseIntegerValue(tableDataNodes[6]);
            var active = ParseIntegerValue(tableDataNodes[7]);
            var serious = ParseIntegerValue(tableDataNodes[8]);

            return new CountryStats(country, totalCases, newCases, totalDeaths, newDeaths, recovered,
                active, serious, fetchedAt);
        }

        private static IEnumerable<HtmlNode> GetTableRows(HtmlDocument document)
            => document
                .DocumentNode
                .Descendants()
                .First(node => node.Id == "main_table_countries_today")
                .ChildNodes
                .First(node => node.Name == "tbody")
                .ChildNodes
                .Where(node => node.GetType() != typeof(HtmlTextNode))
                .ToArray();

        private static IEnumerable<HtmlNode> GetTableDataNodes(HtmlNode htmlNode)
            => htmlNode
                .ChildNodes
                .Where(node => node.Name == "td")
                .ToArray();


        private static string ParseCountry(HtmlNode htmlNode)
        {
            var anchorNode = htmlNode.ChildNodes.FirstOrDefault(node => node.Name == "a");

            if (anchorNode is null)
            {
                var regularNode = htmlNode.InnerHtml.Trim().Replace("&ccedil;", "ç").Replace("&eacute;", "é");

                return regularNode.Contains("<span", StringComparison.InvariantCulture)
                    ? null
                    : regularNode;
            }

            var anchorValue = anchorNode.InnerHtml.Trim().Replace("&ccedil;", "ç").Replace("&eacute", "é");

            return anchorValue.Contains("<span", StringComparison.InvariantCulture) ? null : anchorValue;
        }

        private static int ParseIntegerValue(HtmlNode htmlNode)
        {
            var value = ClearValue(htmlNode.InnerHtml);

            try
            {
                return int.Parse(value, NumberStyles.Any);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static string ClearValue(string value)
        {
            if (value.StartsWith("+-") || value.StartsWith("-+"))
                return 0.ToString();

            if (value == "N/A")
                return 0.ToString();

            return string.IsNullOrWhiteSpace(value) ? 0.ToString() : value;
        }
    }
}