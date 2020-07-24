using System;
using Covid19Api.Domain;
using HtmlAgilityPack;

namespace Covid19Api.Services.Abstractions.Parser
{
    public interface IGlobalStatisticsParser
    {
        GlobalStatistics Parse(HtmlDocument document, DateTime fetchedAt);
    }
}