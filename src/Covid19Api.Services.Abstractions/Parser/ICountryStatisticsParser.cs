using System;
using System.Collections.Generic;
using Covid19Api.Domain;
using HtmlAgilityPack;

namespace Covid19Api.Services.Abstractions.Parser
{
    public interface ICountryStatisticsParser
    {
        IEnumerable<CountryStatistics?> Parse(HtmlDocument document, DateTime fetchedAt);
    }
}