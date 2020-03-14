using System;
using System.Globalization;
using System.Linq;
using Covid19Api.Services.Models;
using HtmlAgilityPack;

namespace Covid19Api.Services
{
    public static class ActiveCasesParser
    {
        public static ActiveCases Parse(HtmlDocument document, DateTime fetchedAt)
        {
            // active cases [0], closed cases [1]
            var activeCases = document
                .DocumentNode
                .Descendants()
                .First(node => node.HasClass("number-table-main"));
            
            
            // active cases, mild:0, serious: 1
            // closed cases, recovered:0, deaths: 1
            var numbersConditions = 
                document
                    .DocumentNode
                    .Descendants()
                    .Where(node => node.HasClass("number-table"))
                    .Take(2)
                    .ToArray();
            
            return new ActiveCases(GetIntegerValue(activeCases), GetIntegerValue(numbersConditions[0]), GetIntegerValue(numbersConditions[1]), fetchedAt);
        }
        
        private static int GetIntegerValue(HtmlNode htmlNode) =>
            int.Parse(htmlNode.InnerHtml, NumberStyles.Any);
    }
}