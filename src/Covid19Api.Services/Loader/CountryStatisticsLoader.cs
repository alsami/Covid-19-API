using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Domain;
using Covid19Api.Services.Abstractions.Loader;
using Covid19Api.Services.Abstractions.Models;
using HtmlAgilityPack;

namespace Covid19Api.Services.Loader
{
    public class CountryStatisticsLoader : ICountryStatisticsLoader
    {
        private readonly ICountryMetaDataLoader countryMetaDataLoader;
        private readonly IHtmlDocumentLoader htmlDocumentLoader;

        private readonly Func<CountryStatistics?, bool> defaultCountryStatisticsFilter = _ => true;

        public CountryStatisticsLoader(ICountryMetaDataLoader countryMetaDataLoader,
            IHtmlDocumentLoader htmlDocumentLoader)
        {
            this.countryMetaDataLoader = countryMetaDataLoader;
            this.htmlDocumentLoader = htmlDocumentLoader;
        }

        public async Task<IEnumerable<CountryStatistics?>> ParseAsync(DateTime fetchedAt,
            Func<CountryStatistics?, bool>? filter = null)
        {
            var document = await this.htmlDocumentLoader.LoadAsync();

            var countryMetaData = await this.countryMetaDataLoader.LoadCountryMetaDataByCountryAsync();

            var countryStatistics = GetTableRows(document)
                .Select(tableRow => Parse(countryMetaData, tableRow, fetchedAt))
                .Where(filter ?? this.defaultCountryStatisticsFilter);

            return countryStatistics;
        }

        private static CountryStatistics? Parse(IEnumerable<CountryMetaData> countryMetaData, HtmlNode htmlNode,
            DateTime fetchedAt)
        {
            var tableDataNodes = GetTableDataNodes(htmlNode).ToArray();

            var country = ParseCountry(tableDataNodes[1]);
            var totalCases = ParseIntegerValue(tableDataNodes[2]);
            var newCases = ParseIntegerValue(tableDataNodes[3]);
            var totalDeaths = ParseIntegerValue(tableDataNodes[4]);
            var newDeaths = ParseIntegerValue(tableDataNodes[5]);
            var recovered = ParseIntegerValue(tableDataNodes[6]);
            var active = ParseIntegerValue(tableDataNodes[8]);
            var serious = ParseIntegerValue(tableDataNodes[9]);

            if (string.IsNullOrWhiteSpace(country)) return null;
            
            var countryCode = GetCountryCode(countryMetaData, country);

            return new CountryStatistics(country, countryCode, totalCases, newCases, totalDeaths, newDeaths, recovered,
                active, serious, fetchedAt);
        }

        private static string? GetCountryCode(IEnumerable<CountryMetaData> countryMetaData, string country)
        {
            var countryCode =
                countryMetaData.FirstOrDefault(metaData =>
                    metaData.Name.StartsWith(country
                            .Replace("UK", "United Kingdom")
                            .Replace("Czechia", "Czech Republic")
                            .Replace("Estwatini", "Kingdom of Eswatini")
                            .Replace("S. Korea", "Korea (Republic of)")
                            .Replace("North Macedonia", "Macedonia (the former Yugoslav Republic of)")
                            .Replace("Vietnam", "Viet Nam")
                            .Replace("Vatican City", "Holy See")
                        ,
                        StringComparison.InvariantCultureIgnoreCase) ||
                    metaData.AltSpellings.Contains(country, StringComparer.InvariantCultureIgnoreCase))?.Alpha2Code;
            
            return countryCode;
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


        private static string? ParseCountry(HtmlNode htmlNode)
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
            var value = ClearValue(htmlNode.InnerHtml)
                .Replace(",", "");

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