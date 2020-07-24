using System;
using HtmlAgilityPack;
using MediatR;

namespace Covid19Api.UseCases.Abstractions.Commands
{
    public class RefreshCountriesStatisticsCommand : IRequest
    {
        public RefreshCountriesStatisticsCommand(DateTime fetchedAt, HtmlDocument document)
        {
            this.FetchedAt = fetchedAt;
            this.Document = document;
        }

        public DateTime FetchedAt { get; }

        public HtmlDocument Document { get; }
    }
}